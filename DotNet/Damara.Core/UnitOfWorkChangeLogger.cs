// <copyright file="UnitOfWorkChangeLogger.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Serilog;

namespace Damara;

/// <summary>
/// Logs the entities affected by every save and cancel performed on a unit of work.
/// The unit of work events are weak, so the subscriber must keep a strong reference to
/// this instance (for example in a field) for the lifetime of the unit of work.
/// </summary>
public sealed class UnitOfWorkChangeLogger
{
    /// <summary>
    /// Upper bound on the entity descriptions attached to one log event per kind of change.
    /// Serilog's Seq sink drops any event whose JSON exceeds its body size limit (256 KB by default)
    /// and writes a placeholder in its place, so a save that touches thousands of rows must not list
    /// every one of them. The counts in the message stay exact; the list ends with a per-type summary
    /// of what was left out.
    /// </summary>
    public const int MaxDescribedEntities = 100;

    private readonly UnitOfWorkBase unitOfWork;
    private readonly Func<ILogger> loggerAccessor;

    private EntityBase[] pendingAdded = [];
    private EntityBase[] pendingModified = [];
    private EntityBase[] pendingDeleted = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWorkChangeLogger"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work to observe.</param>
    /// <param name="loggerAccessor">Accessor returning the logger to write to, evaluated at the time of each save or cancel.</param>
    public UnitOfWorkChangeLogger(UnitOfWorkBase unitOfWork, Func<ILogger> loggerAccessor)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(loggerAccessor);

        this.unitOfWork = unitOfWork;
        this.loggerAccessor = loggerAccessor;

        this.unitOfWork.BeforeSaveChanges += this.HandleBeforeSaveChanges;
        this.unitOfWork.AfterSaveChanges += this.HandleAfterSaveChanges;
        this.unitOfWork.BeforeCancelChanges += this.HandleBeforeCancelChanges;
        this.unitOfWork.AfterCancelChanges += this.HandleAfterCancelChanges;
    }

    /// <summary>
    /// Describes the entities as "Type: ToString" strings, listing at most
    /// <see cref="MaxDescribedEntities"/> of them. When more were changed, a final entry
    /// states how many were left out and how they split by entity type.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>One description per listed entity, plus the summary entry when truncated.</returns>
    public static string[] Describe(EntityBase[] entities)
    {
        var described = entities
            .Take(MaxDescribedEntities)
            .Select(entity => string.Format("{0}: {1}", GetEntityTypeName(entity), entity));

        if (entities.Length <= MaxDescribedEntities)
        {
            return described.ToArray();
        }

        var leftOutByType = entities
            .Skip(MaxDescribedEntities)
            .GroupBy(GetEntityTypeName)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group => string.Format("{0} x{1}", group.Key, group.Count()));

        var summary = string.Format(
            "... and {0} more ({1})",
            entities.Length - MaxDescribedEntities,
            string.Join(", ", leftOutByType));

        return described.Append(summary).ToArray();
    }

    /// <summary>
    /// Gets the name of the entity's model type. Entities loaded through EF Core lazy-loading
    /// proxies are instances of a generated subclass in the <c>Castle.Proxies</c> namespace,
    /// so the proxy's base type is reported instead.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The entity type name.</returns>
    private static string GetEntityTypeName(EntityBase entity)
    {
        var type = entity.GetType();
        return type.Namespace == "Castle.Proxies" && type.BaseType != null ? type.BaseType.Name : type.Name;
    }

    /// <summary>
    /// Captures the pending changes so they can be reported once the save completes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The event arguments.</param>
    private void HandleBeforeSaveChanges(object sender, BeforeSaveChangesArgs args)
    {
        this.pendingAdded = this.unitOfWork.GetAddedEntities().ToArray();
        this.pendingModified = this.unitOfWork.GetModifiedEntities().ToArray();
        this.pendingDeleted = this.unitOfWork.GetDeletedEntities().ToArray();
    }

    /// <summary>
    /// Logs the changes captured before the save.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The event arguments.</param>
    private void HandleAfterSaveChanges(object sender, AfterSaveChangesArgs args)
    {
        this.LogChanges("saved", this.pendingAdded, this.pendingModified, this.pendingDeleted);

        this.pendingAdded = [];
        this.pendingModified = [];
        this.pendingDeleted = [];
    }

    /// <summary>
    /// Captures and describes the pending changes before they are discarded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The event arguments.</param>
    private void HandleBeforeCancelChanges(object sender, BeforeCancelChangesArgs args)
    {
        this.pendingAdded = this.unitOfWork.GetAddedEntities().ToArray();
        this.pendingModified = this.unitOfWork.GetModifiedEntities().ToArray();
        this.pendingDeleted = this.unitOfWork.GetDeletedEntities().ToArray();
    }

    /// <summary>
    /// Logs the changes captured before the cancel.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The event arguments.</param>
    private void HandleAfterCancelChanges(object sender, AfterCancelChangesArgs args)
    {
        this.LogChanges("cancelled", this.pendingAdded, this.pendingModified, this.pendingDeleted);

        this.pendingAdded = [];
        this.pendingModified = [];
        this.pendingDeleted = [];
    }

    /// <summary>
    /// Logs the supplied changes: exact counts in the message, and a capped description list
    /// per kind of change as event properties.
    /// </summary>
    /// <param name="operation">The operation performed, e.g. "saved" or "cancelled".</param>
    /// <param name="added">The added entities.</param>
    /// <param name="modified">The modified entities.</param>
    /// <param name="deleted">The deleted entities.</param>
    private void LogChanges(string operation, EntityBase[] added, EntityBase[] modified, EntityBase[] deleted)
    {
        var logger = this.loggerAccessor();
        if (logger == null)
        {
            return;
        }

        if (added.Length == 0 && modified.Length == 0 && deleted.Length == 0)
        {
            logger.Debug("Changes {Operation} with no pending changes", operation);
            return;
        }

        if (added.Length > 0)
        {
            logger = logger.ForContext("AddedEntities", Describe(added));
        }

        if (modified.Length > 0)
        {
            logger = logger.ForContext("ModifiedEntities", Describe(modified));
        }

        if (deleted.Length > 0)
        {
            logger = logger.ForContext("DeletedEntities", Describe(deleted));
        }

        logger.Information(
            "Changes {Operation}: {AddedCount} added, {ModifiedCount} modified, {DeletedCount} deleted",
            operation,
            added.Length,
            modified.Length,
            deleted.Length);
    }
}
