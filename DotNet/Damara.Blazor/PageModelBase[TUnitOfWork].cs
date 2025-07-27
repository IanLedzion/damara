// <copyright file="PageModelBase[TUnitOfWork].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using ILogger = Serilog.ILogger;

namespace Damara.Blazor;

/// <summary>
/// Provides a base page model class.
/// </summary>
/// <typeparam name="TUnitOfWork">Type of the unit of work.</typeparam>
public abstract class PageModelBase<TUnitOfWork> : ServiceBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageModelBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    protected PageModelBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
    }
}