// <copyright file="SubscribingServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Serilog;

namespace Damara;

/// <summary>
/// Provides a base class for all service types.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
public abstract class SubscribingServiceBase<TUnitOfWork> : ServiceBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    private readonly List<Action> beforeChangesSavedActions;
    private readonly List<Action> beforeChangesCanceledActions;
    private readonly List<Action> afterChangesSavedActions;
    private readonly List<Action> afterChangesCanceledActions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribingServiceBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    protected SubscribingServiceBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
        this.beforeChangesSavedActions = [];
        this.beforeChangesCanceledActions = [];
        this.afterChangesSavedActions = [];
        this.afterChangesCanceledActions = [];

        this.UnitOfWork.BeforeSaveChanges += this.HandleBeforeSaveChanges;
        this.UnitOfWork.AfterSaveChanges += this.HandleAfterSaveChanges;
        this.UnitOfWork.BeforeCancelChanges += this.HandleBeforeCancelChanges;
        this.UnitOfWork.AfterCancelChanges += this.HandleAfterCancelChanges;
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected override void OnBeforeDisposing(bool disposing)
    {
        if (disposing)
        {
            this.UnitOfWork.BeforeSaveChanges -= this.HandleBeforeSaveChanges;
            this.UnitOfWork.AfterSaveChanges -= this.HandleAfterSaveChanges;
            this.UnitOfWork.BeforeCancelChanges -= this.HandleBeforeCancelChanges;
            this.UnitOfWork.AfterCancelChanges -= this.HandleAfterCancelChanges;

            this.beforeChangesSavedActions.Clear();
            this.afterChangesSavedActions.Clear();
            this.beforeChangesCanceledActions.Clear();
            this.afterChangesCanceledActions.Clear();
        }
    }

    /// <summary>
    /// Occurs before changes are saved.
    /// </summary>
    protected virtual void OnBeforeSaveChanges()
    {
    }

    /// <summary>
    /// Occurs after changes are saved.
    /// </summary>
    protected virtual void OnAfterSaveChanges()
    {
    }

    /// <summary>
    /// Occurs before changes are canceled.
    /// </summary>
    protected virtual void OnBeforeCancelChanges()
    {
    }

    /// <summary>
    /// Occurs after changes are canceled.
    /// </summary>
    protected virtual void OnAfterCancelChanges()
    {
    }

    /// <summary>
    /// Adds the before save changes action.
    /// </summary>
    /// <param name="action">The action.</param>
    protected void AddBeforeSaveChangesAction(Action action)
    {
        this.beforeChangesSavedActions.Add(action);
    }

    /// <summary>
    /// Adds the before cancel changes action.
    /// </summary>
    /// <param name="action">The action.</param>
    protected void AddBeforeCancelChangesAction(Action action)
    {
        this.beforeChangesCanceledActions.Add(action);
    }

    /// <summary>
    /// Adds the after save changes action.
    /// </summary>
    /// <param name="action">The action.</param>
    protected void AddAfterSaveChangesAction(Action action)
    {
        this.afterChangesSavedActions.Add(action);
    }

    /// <summary>
    /// Adds the after cancel changes action.
    /// </summary>
    /// <param name="action">The action.</param>
    protected void AddAfterCancelChangesAction(Action action)
    {
        this.afterChangesCanceledActions.Add(action);
    }

    /// <summary>
    /// Handles the before save changes event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    private void HandleBeforeSaveChanges(object sender, BeforeSaveChangesArgs args)
    {
        this.OnBeforeSaveChanges();

        this.beforeChangesSavedActions.ForEach(action => action.Invoke());
        this.beforeChangesSavedActions.Clear();
    }

    /// <summary>
    /// Handles the after save changes event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    private void HandleAfterSaveChanges(object sender, AfterSaveChangesArgs args)
    {
        this.afterChangesSavedActions.ForEach(action => action.Invoke());
        this.afterChangesSavedActions.Clear();

        this.OnAfterSaveChanges();
    }

    /// <summary>
    /// Handles the before cancel changes event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    private void HandleBeforeCancelChanges(object sender, BeforeCancelChangesArgs args)
    {
        this.OnBeforeCancelChanges();

        this.beforeChangesCanceledActions.ForEach(action => action.Invoke());
        this.beforeChangesCanceledActions.Clear();
    }

    /// <summary>
    /// Handles the after cancel changes event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The arguments.</param>
    private void HandleAfterCancelChanges(object sender, AfterCancelChangesArgs args)
    {
        this.afterChangesCanceledActions.ForEach(action => action.Invoke());
        this.afterChangesCanceledActions.Clear();

        this.OnAfterCancelChanges();
    }
}