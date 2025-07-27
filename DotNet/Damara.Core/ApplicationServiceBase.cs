// <copyright file="ApplicationServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Serilog;

namespace Damara;

/// <summary>
/// Base class for applicagtion services.
/// </summary>
public abstract class ApplicationServiceBase<TUnitOfWork> : SubscribingServiceBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationServiceBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    protected ApplicationServiceBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
    }
}