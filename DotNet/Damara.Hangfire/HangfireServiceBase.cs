// <copyright file="HangfireServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Serilog;

namespace Damara.Hangfire;

/// <summary>
/// Provides a base class for Hangfire services.
/// </summary>
public abstract class HangfireServiceBase<TUnitOfWork> : ServiceBase<TUnitOfWork>
       where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireServiceBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    protected HangfireServiceBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
    }
}