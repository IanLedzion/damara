// <copyright file="ReadOnlyServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Represents a read-only service.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class ReadOnlyServiceBase<TUnitOfWork, TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyServiceBase{TUnitOfWork, TEntity}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="entity">The entity.</param>
    protected ReadOnlyServiceBase(TUnitOfWork unitOfWork, TEntity entity)
    {
        this.UnitOfWork = unitOfWork;
        this.Entity = entity;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    public TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    public TEntity Entity { get; }
}