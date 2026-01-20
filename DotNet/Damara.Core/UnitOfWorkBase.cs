// <copyright file="UnitOfWorkBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using WeakEvent;

namespace Damara;

/// <summary>
/// Provides a base class for unit of work instances.
/// </summary>
public abstract class UnitOfWorkBase : IDisposable, IUnitOfWork
{
    /// <summary>
    /// The sequence.
    /// </summary>
    private static int sequence = 0;

    private readonly WeakEventSource<BeforeSaveChangesArgs> beforeSaveChanges = new();
    private readonly WeakEventSource<AfterSaveChangesArgs> afterSaveChanges = new();
    private readonly WeakEventSource<BeforeCancelChangesArgs> beforeCancelChanges = new();
    private readonly WeakEventSource<AfterCancelChangesArgs> afterCancelChanges = new();

    /// <summary>
    /// Indicates whether the instance has been disposed.
    /// </summary>
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWorkBase"/> class.
    /// </summary>
    protected UnitOfWorkBase()
    {
        this.CreateSequence = ++sequence;
    }

    /// <summary>
    /// Occurs before changes are saved.
    /// </summary>
    public event EventHandler<BeforeSaveChangesArgs> BeforeSaveChanges { add => this.beforeSaveChanges.Subscribe(value); remove => this.beforeSaveChanges.Unsubscribe(value); }

    /// <summary>
    /// Occurs after changes are saved.
    /// </summary>
    public event EventHandler<AfterSaveChangesArgs> AfterSaveChanges { add => this.afterSaveChanges.Subscribe(value); remove => this.afterSaveChanges.Unsubscribe(value); }

    /// <summary>
    /// Occurs before changes are cancelled.
    /// </summary>
    public event EventHandler<BeforeCancelChangesArgs> BeforeCancelChanges { add => this.beforeCancelChanges.Subscribe(value); remove => this.beforeCancelChanges.Unsubscribe(value); }

    /// <summary>
    /// Occurs after changes are cancelled.
    /// </summary>
    public event EventHandler<AfterCancelChangesArgs> AfterCancelChanges { add => this.afterCancelChanges.Subscribe(value); remove => this.afterCancelChanges.Unsubscribe(value); }

    /// <summary>
    /// Gets the create sequence.
    /// </summary>
    public int CreateSequence { get; }

    /// <summary>
    /// Gets a value indicating whether this instance has changes.
    /// </summary>
    public bool HasChanges => !this.disposed && this.HasChangesCore;

    /// <summary>
    /// Gets a value indicating whether this instance has changes.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has changes; otherwise, <c>false</c>.
    /// </value>
    protected abstract bool HasChangesCore { get; }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    public void SaveChanges()
    {
        this.beforeSaveChanges?.Raise(this, new BeforeSaveChangesArgs());
        this.SaveChangesCore();
        this.afterSaveChanges?.Raise(this, new AfterSaveChangesArgs());
    }

    /// <summary>
    /// Cancels the changes.
    /// </summary>
    public void CancelChanges()
    {
        this.beforeCancelChanges?.Raise(this, new BeforeCancelChangesArgs());
        this.CancelChangesCore();
        this.afterCancelChanges?.Raise(this, new AfterCancelChangesArgs());
    }

    /// <summary>
    /// Refreshes the selected entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    public void Refresh(IEnumerable<EntityBase> entities)
    {
        if (this.disposed)
        {
            return;
        }

        this.RefreshCore(entities);
    }

    /// <summary>
    /// Gets the added entities.
    /// </summary>
    /// <returns>A collection of added entities.</returns>
    public IEnumerable<EntityBase> GetAddedEntities()
    {
        if (this.disposed)
        {
            return [];
        }

        return this.GetAddedEntitiesCore();
    }

    /// <summary>
    /// Gets the modified entities.
    /// </summary>
    /// <returns>A collection of modified entities.</returns>
    public IEnumerable<EntityBase> GetModifiedEntities()
    {
        if (this.disposed)
        {
            return [];
        }

        return this.GetModifiedEntitiesCore();
    }

    /// <summary>
    /// Gets the deleted entities.
    /// </summary>
    /// <returns>A collection of deleted entities.</returns>
    public IEnumerable<EntityBase> GetDeletedEntities()
    {
        if (this.disposed)
        {
            return [];
        }

        return this.GetDeletedEntitiesCore();
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return string.Format("Sequence={0}", this.CreateSequence);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Called before validation occurs.
    /// </summary>
    void IUnitOfWork.OnBeforeValidate()
    {
        this.OnBeforeValidateCore();
    }

    /// <summary>
    /// Core before validation method.
    /// </summary>
    protected virtual void OnBeforeValidateCore()
    {
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        this.disposed = true;
    }

    /// <summary>
    /// Core save changes method.
    /// </summary>
    protected abstract void SaveChangesCore();

    /// <summary>
    /// Core cancel changes method.
    /// </summary>
    protected abstract void CancelChangesCore();

    /// <summary>
    /// Refreshes the selected entities (core method).
    /// </summary>
    /// <param name="entities">The entities.</param>
    protected abstract void RefreshCore(IEnumerable<EntityBase> entities);

    /// <summary>
    /// Gets the added entities (core method).
    /// </summary>
    /// <returns>A collection of added entities.</returns>
    protected abstract IEnumerable<EntityBase> GetAddedEntitiesCore();

    /// <summary>
    /// Gets the modified entities (core method).
    /// </summary>
    /// <returns>A collection of modified entities.</returns>
    protected abstract IEnumerable<EntityBase> GetModifiedEntitiesCore();

    /// <summary>
    /// Gets the deleted entities (core method).
    /// </summary>
    /// <returns>A collection of deleted entities.</returns>
    protected abstract IEnumerable<EntityBase> GetDeletedEntitiesCore();
}