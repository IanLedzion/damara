// <copyright file="RepositoryBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides a base class for repositories.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The entity type managed by the repository.</typeparam>
public abstract class RepositoryBase<TUnitOfWork, TEntity> : IRepository<TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{TUnitOfWork, TEntity}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected RepositoryBase(TUnitOfWork unitOfWork)
    {
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public abstract void Add(TEntity entity);

    /// <summary>
    /// Removes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public abstract void Remove(TEntity entity);

    /// <summary>
    /// Refreshes the specified collection of entities in the repository.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="entities">The entities.</param>
    public abstract void Refresh(IEnumerable<TEntity> entities);

    /// <summary>
    /// Refreshes the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public abstract void Refresh(TEntity entity);

    /// <summary>
    /// Gets all the entities in the current repository.
    /// </summary>
    public abstract IQueryable<TEntity> GetAll();
}