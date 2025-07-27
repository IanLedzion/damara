// <copyright file="ServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Serilog;

namespace Damara;
#nullable enable

/// <summary>
/// Provides a base class for all service types.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
public abstract class ServiceBase<TUnitOfWork> : LoggableBase
    where TUnitOfWork : IUnitOfWork
{
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="ArgumentNullException">unitOfWork - Unit of work cannot be null.</exception>
    protected ServiceBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(logger)
    {
        this.UnitOfWork = unitOfWork;
        this.Logger.Debug("Service instance {Service} initialised", this.GetType().FullName);
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        // Dispose of unmanaged resources.
        this.Dispose(true);

        // Suppress finalization.
        GC.SuppressFinalize(this);

        this.Logger.Debug("Service instance {Service} disposed", this.GetType().FullName);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        this.OnBeforeDisposing(disposing);

        this.disposed = true;
    }

    /// <summary>
    /// Called before before disposing.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void OnBeforeDisposing(bool disposing)
    {
    }
}