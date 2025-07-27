// <copyright file="DomainServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides a base class for domain service types.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
public class DomainServiceBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainServiceBase{TUnitOfWork}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected DomainServiceBase(TUnitOfWork unitOfWork)
    {
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }
}