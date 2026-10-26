// <copyright file="ChangeTrackingMockUnitOfWork.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Unit of work double whose pending changes are set by the test and whose save and cancel
/// do nothing but raise the events, so subscribers such as the change logger can be exercised
/// without a data context.
/// </summary>
public class ChangeTrackingMockUnitOfWork : UnitOfWorkBase
{
    /// <summary>
    /// Gets the entities reported as added.
    /// </summary>
    public List<EntityBase> Added { get; } = [];

    /// <summary>
    /// Gets the entities reported as modified.
    /// </summary>
    public List<EntityBase> Modified { get; } = [];

    /// <summary>
    /// Gets the entities reported as deleted.
    /// </summary>
    public List<EntityBase> Deleted { get; } = [];

    protected override bool HasChangesCore => this.Added.Count + this.Modified.Count + this.Deleted.Count > 0;

    [Obsolete("EF Core change tracking does not support undo; kept for backwards compatibility.")]
    protected override void CancelChangesCore()
    {
    }

    protected override IEnumerable<EntityBase> GetAddedEntitiesCore() => this.Added;

    protected override IEnumerable<EntityBase> GetDeletedEntitiesCore() => this.Deleted;

    protected override IEnumerable<EntityBase> GetModifiedEntitiesCore() => this.Modified;

    protected override void RefreshCore(IEnumerable<EntityBase> entities)
    {
    }

    protected override void SaveChangesCore()
    {
    }
}
