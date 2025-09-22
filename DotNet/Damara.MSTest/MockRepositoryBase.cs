// <copyright file="MockRepositoryBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.MSTest;

/// <summary>
/// Base class for mock repositories.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="Damara.IRepository&lt;TEntity&gt;" />
public abstract class MockRepositoryBase<TUnitOfWork, TEntity> : IRepository<TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    private readonly List<TEntity> entities;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockRepositoryBase{TUnitOfWork, TEntity}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="entities">The entities.</param>
    protected MockRepositoryBase(TUnitOfWork unitOfWork, IEnumerable<TEntity> entities)
    {
        this.UnitOfWork = unitOfWork;
        this.entities = entities.ToList();
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Add(TEntity entity) => this.entities.Add(entity);

    public IEnumerable<TEntity> GetAll() => this.entities;

    /// <summary>
    /// Refreshes the specified collection of entities in the repository.
    /// </summary>
    /// <param name="entities">The entities.</param>
    public void Refresh(IEnumerable<TEntity> entities)
    {
    }

    /// <summary>
    /// Refreshes the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Refresh(TEntity entity)
    {
    }

    /// <summary>
    /// Removes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Remove(TEntity entity) => this.entities.Remove(entity);
}
