// <copyright file="UnitOfWorkChangeLoggerTests.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Damara.Core.Tests.ObjectInstances;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Damara.Core.Tests;

/// <summary>
/// Tests for <see cref="UnitOfWorkChangeLogger"/>, in particular the cap on the entity
/// descriptions attached to a change event so that the Seq sink never drops the event.
/// </summary>
[TestClass]
public class UnitOfWorkChangeLoggerTests
{
    /// <summary>
    /// A save under the cap lists every entity and reports the exact count.
    /// </summary>
    [TestMethod]
    public void SaveChanges_UnderCap_ListsEveryEntity()
    {
        var events = new List<LogEvent>();
        using var unitOfWork = new ChangeTrackingMockUnitOfWork();
        var logger = new UnitOfWorkChangeLogger(unitOfWork, () => CreateLogger(events));
        unitOfWork.Added.AddRange(Enumerable.Range(0, 3).Select(i => new NamedEntity($"Item {i}")));
        unitOfWork.Modified.Add(new NamedEntity("Changed"));

        unitOfWork.SaveChanges();

        var changeEvent = events.Single();
        Assert.AreEqual("saved", GetScalar(changeEvent, "Operation"), "Operation not logged");
        Assert.AreEqual(3, GetScalar(changeEvent, "AddedCount"), "Added count wrong");
        Assert.AreEqual(1, GetScalar(changeEvent, "ModifiedCount"), "Modified count wrong");
        Assert.AreEqual(0, GetScalar(changeEvent, "DeletedCount"), "Deleted count wrong");
        CollectionAssert.AreEqual(
            new[] { "NamedEntity: Item 0", "NamedEntity: Item 1", "NamedEntity: Item 2" },
            GetSequence(changeEvent, "AddedEntities"),
            "Added entities not listed in full");
        CollectionAssert.AreEqual(new[] { "NamedEntity: Changed" }, GetSequence(changeEvent, "ModifiedEntities"), "Modified entities not listed");
        Assert.IsFalse(changeEvent.Properties.ContainsKey("DeletedEntities"), "An empty list must not be attached");
    }

    /// <summary>
    /// A save over the cap keeps the exact count in the message but lists only the first
    /// <see cref="UnitOfWorkChangeLogger.MaxDescribedEntities"/> entities, closing with a
    /// per-type summary of the rest, so the event stays far below the Seq body size limit.
    /// </summary>
    [TestMethod]
    public void SaveChanges_OverCap_TruncatesWithSummary()
    {
        var events = new List<LogEvent>();
        using var unitOfWork = new ChangeTrackingMockUnitOfWork();
        var logger = new UnitOfWorkChangeLogger(unitOfWork, () => CreateLogger(events));
        var cap = UnitOfWorkChangeLogger.MaxDescribedEntities;
        unitOfWork.Added.AddRange(Enumerable.Range(0, cap + 150).Select(i => new NamedEntity($"Item {i}")));
        unitOfWork.Added.AddRange(Enumerable.Range(0, 40).Select(i => new EntityInstance { Name = $"Group {i}" }));

        unitOfWork.SaveChanges();

        var changeEvent = events.Single();
        var added = GetSequence(changeEvent, "AddedEntities");
        Assert.AreEqual(cap + 190, GetScalar(changeEvent, "AddedCount"), "The count must stay exact");
        Assert.HasCount(cap + 1, added, "The list must hold the cap plus one summary entry");
        Assert.AreEqual("NamedEntity: Item 0", added[0], "The first entity must be listed");
        Assert.AreEqual($"NamedEntity: Item {cap - 1}", added[cap - 1], "The last listed entity must be the one at the cap");
        Assert.AreEqual("... and 190 more (NamedEntity x150, EntityInstance x40)", added[cap], "The summary must count the left-out entities by type");
    }

    /// <summary>
    /// A cancel is reported the same way as a save, under the "cancelled" operation.
    /// </summary>
    [TestMethod]
    public void CancelChanges_WithPendingDeletes_LogsCancelled()
    {
        var events = new List<LogEvent>();
        using var unitOfWork = new ChangeTrackingMockUnitOfWork();
        var logger = new UnitOfWorkChangeLogger(unitOfWork, () => CreateLogger(events));
        unitOfWork.Deleted.Add(new NamedEntity("Gone"));

#pragma warning disable CS0618 // CancelChanges is obsolete under EF Core but still raises the cancel events the logger reports.
        unitOfWork.CancelChanges();
#pragma warning restore CS0618

        var changeEvent = events.Single();
        Assert.AreEqual("cancelled", GetScalar(changeEvent, "Operation"), "Operation not logged");
        Assert.AreEqual(1, GetScalar(changeEvent, "DeletedCount"), "Deleted count wrong");
        CollectionAssert.AreEqual(new[] { "NamedEntity: Gone" }, GetSequence(changeEvent, "DeletedEntities"), "Deleted entities not listed");
    }

    /// <summary>
    /// A save with nothing pending writes only a debug line and attaches no lists.
    /// </summary>
    [TestMethod]
    public void SaveChanges_NoChanges_LogsDebugOnly()
    {
        var events = new List<LogEvent>();
        using var unitOfWork = new ChangeTrackingMockUnitOfWork();
        var logger = new UnitOfWorkChangeLogger(unitOfWork, () => CreateLogger(events));

        unitOfWork.SaveChanges();

        var changeEvent = events.Single();
        Assert.AreEqual(LogEventLevel.Debug, changeEvent.Level, "No-change saves must stay at debug level");
        Assert.IsFalse(changeEvent.Properties.ContainsKey("AddedEntities"), "No list must be attached");
    }

    /// <summary>
    /// Builds a logger that collects every event, at all levels, into the given list.
    /// </summary>
    /// <param name="events">The list receiving the events.</param>
    /// <returns>The logger.</returns>
    private static ILogger CreateLogger(List<LogEvent> events) =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Sink(new CollectingSink(events))
            .CreateLogger();

    /// <summary>
    /// Reads a scalar property from a log event.
    /// </summary>
    /// <param name="logEvent">The event.</param>
    /// <param name="name">The property name.</param>
    /// <returns>The property value.</returns>
    private static object GetScalar(LogEvent logEvent, string name) => ((ScalarValue)logEvent.Properties[name]).Value;

    /// <summary>
    /// Reads a sequence-of-strings property from a log event.
    /// </summary>
    /// <param name="logEvent">The event.</param>
    /// <param name="name">The property name.</param>
    /// <returns>The strings in order.</returns>
    private static string[] GetSequence(LogEvent logEvent, string name) =>
        ((SequenceValue)logEvent.Properties[name]).Elements.Select(element => (string)((ScalarValue)element).Value).ToArray();

    /// <summary>
    /// Entity whose ToString is its name, so descriptions read "NamedEntity: name".
    /// </summary>
    private sealed class NamedEntity(string name) : EntityBase
    {
        /// <inheritdoc/>
        public override string ToString() => name;
    }

    /// <summary>
    /// Sink appending every event to a list.
    /// </summary>
    private sealed class CollectingSink(List<LogEvent> events) : ILogEventSink
    {
        /// <inheritdoc/>
        public void Emit(LogEvent logEvent) => events.Add(logEvent);
    }
}
