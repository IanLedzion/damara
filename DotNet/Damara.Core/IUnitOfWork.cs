// <copyright file="IUnitOfWork.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Contract for the unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Occurs before changes are saved.
    /// </summary>
    public event EventHandler<BeforeSaveChangesArgs> BeforeSaveChanges;

    /// <summary>
    /// Occurs after changes are saved.
    /// </summary>
    public event EventHandler<AfterSaveChangesArgs> AfterSaveChanges;

    /// <summary>
    /// Occurs before changes are cancelled.
    /// </summary>
    public event EventHandler<BeforeCancelChangesArgs> BeforeCancelChanges;

    /// <summary>
    /// Occurs after changes are cancelled.
    /// </summary>
    public event EventHandler<AfterCancelChangesArgs> AfterCancelChanges;

    /// <summary>
    /// Gets the create sequence.
    /// </summary>
    public int CreateSequence { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has changes.
    /// </summary>
    public bool HasChanges { get; }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    public void SaveChanges();

    /// <summary>
    /// Cancels the changes.
    /// </summary>
    public void CancelChanges();

    /// <summary>
    /// Refreshes the selected entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    public void Refresh(IEnumerable<EntityBase> entities);

    /// <summary>
    /// Gets the added entities.
    /// </summary>
    /// <returns>A collection of added entities.</returns>
    public IEnumerable<EntityBase> GetAddedEntities();

    /// <summary>
    /// Gets the modified entities.
    /// </summary>
    /// <returns>A collection of modified entities.</returns>
    public IEnumerable<EntityBase> GetModifiedEntities();

    /// <summary>
    /// Gets the deleted entities.
    /// </summary>
    /// <returns>A collection of deleted entities.</returns>
    public IEnumerable<EntityBase> GetDeletedEntities();

    /// <summary>
    /// Called before validation occurs.
    /// </summary>
    internal void OnBeforeValidate();
}