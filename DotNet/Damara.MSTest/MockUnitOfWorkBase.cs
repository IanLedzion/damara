// <copyright file="MockUnitOfWorkBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.MSTest;

/// <summary>
/// Mock unit of work.
/// </summary>
/// <seealso cref="Damara.UnitOfWorkBase" />
public abstract class MockUnitOfWorkBase : UnitOfWorkBase
{
    /// <summary>
    /// Gets a value indicating whether this instance has changes.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance has changes; otherwise, <c>false</c>.
    /// </value>
    protected override bool HasChangesCore => false;

    /// <summary>
    /// Gets the added entities.
    /// </summary>
    /// <returns>
    /// A collection of added entities.
    /// </returns>
    protected override IEnumerable<EntityBase> GetAddedEntitiesCore() => [];

    /// <summary>
    /// Gets the modified entities.
    /// </summary>
    /// <returns>
    /// A collection of modified entities.
    /// </returns>
    protected override IEnumerable<EntityBase> GetModifiedEntitiesCore() => [];

    /// <summary>
    /// Gets the deleted entities.
    /// </summary>
    /// <returns>
    /// A collection of deleted entities.
    /// </returns>
    protected override IEnumerable<EntityBase> GetDeletedEntitiesCore() => [];

    /// <summary>
    /// Refreshes the selected entities (core method).
    /// </summary>
    /// <param name="entities">The entities.</param>
    protected override void RefreshCore(IEnumerable<EntityBase> entities)
    {
    }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    protected override void SaveChangesCore()
    {
    }

    /// <summary>
    /// Cancels the changes.
    /// </summary>
    protected override void CancelChangesCore()
    {
    }
}
