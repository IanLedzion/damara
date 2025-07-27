// <copyright file="EntityFrameworkUnitOfWork.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Damara.EntityFramework;

/// <summary>
/// Provides a strongly typed unit of work for working with LinqConnect.
/// </summary>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
public abstract class EntityFrameworkUnitOfWork<TDbContext> : UnitOfWorkBase
    where TDbContext : DbContext
{
    /// <summary>
    /// Indicates whether the instance has been disposed.
    /// </summary>
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkUnitOfWork{TDataContext}"/> class.
    /// </summary>
    /// <param name="connectionStringName">Name of the connection string.</param>
    /// <exception cref="InvalidOperationException">No connection string was found either in the configuration file or in the UnitOfWorkFactory.</exception>
    protected EntityFrameworkUnitOfWork(TDbContext dbContext)
    {
        this.Context = dbContext;
    }

    /// <summary>
    /// Gets the database context.
    /// </summary>
    public TDbContext Context { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance has changes.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has changes; otherwise, <c>false</c>.
    /// </value>
    protected override bool HasChangesCore => this.Context.ChangeTracker.HasChanges();

    /// <summary>
    /// Refreshes this instance.
    /// </summary>
    /// <param name="entities">The entities.</param>
    protected override void RefreshCore(IEnumerable<EntityBase> entities)
    {
        foreach (var entity in entities)
        {
            this.Context.Entry(entity).Reload();
        }
    }

    /// <summary>
    /// Gets the added entities.
    /// </summary>
    /// <returns>
    /// A collection of added entities.
    /// </returns>
    protected override IEnumerable<EntityBase> GetAddedEntitiesCore() =>
        this.Context
        .ChangeTracker
        .Entries()
        .Where(entry => entry.State == EntityState.Added)
        .Select(entry => entry.Entity)
        .OfType<EntityBase>();

    /// <summary>
    /// Gets the modified entities.
    /// </summary>
    /// <returns>
    /// A collection of modified entities.
    /// </returns>
    protected override IEnumerable<EntityBase> GetModifiedEntitiesCore() =>
        this.Context
        .ChangeTracker
        .Entries()
        .Where(entry => entry.State == EntityState.Modified)
        .Select(entry => entry.Entity)
        .OfType<EntityBase>();

    /// <summary>
    /// Gets the deleted entities.
    /// </summary>
    /// <returns>
    /// A collection of deleted entities.
    /// </returns>
    protected override IEnumerable<EntityBase> GetDeletedEntitiesCore() =>
        this.Context
        .ChangeTracker
        .Entries()
        .Where(entry => entry.State == EntityState.Deleted)
        .Select(entry => entry.Entity)
        .OfType<EntityBase>();

    /// <summary>
    /// Saves the changes.
    /// </summary>
    protected override void SaveChangesCore()
    {
        Log.Verbose("Unit of work {UnitOfWorkType} saving changes", this.GetType().Name);

        try
        {
            this.Context.SaveChanges();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception occured while unit of work {UnitOfWorkType} was saving changes", this.GetType().Name);
            throw;
        }

        Log.Verbose("Unit of work {UnitOfWorkType} changes saved", this.GetType().Name);
    }

    /// <summary>
    /// Cancels the changes.
    /// </summary>
    protected override void CancelChangesCore()
    {
        Log.Verbose("Unit of work {UnitOfWorkType} cancelling changes", this.GetType().Name);

        var changedEntries =
            this.Context
            .ChangeTracker
            .Entries()
            .Where(x => x.State != EntityState.Unchanged)
            .ToList();

        foreach (var entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }

        Log.Verbose("Unit of work {UnitOfWorkType} changes cancelled", this.GetType().Name);
    }

    /// <summary>
    /// Closes the database context.
    /// </summary>
    protected virtual void CloseDataContext()
    {
        if (this.Context != null)
        {
            this.Context.Dispose();
            this.Context = null;
        }
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!this.disposed)
        {
            if (disposing)
            {
                this.CloseDataContext();
            }
        }

        this.disposed = true;
    }
}