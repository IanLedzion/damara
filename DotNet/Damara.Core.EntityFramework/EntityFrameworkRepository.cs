// <copyright file="EntityFrameworkRepository.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace Damara.EntityFramework;

/// <summary>
/// Provides a concrete repository implementation for EntityFramework database contexts.
/// </summary>
/// <typeparam name="TDbContext">The type of the database context.</typeparam>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The entity type managed by the repository.</typeparam>
public abstract class EntityFrameworkRepository<TDbContext, TUnitOfWork, TEntity> : RepositoryBase<TUnitOfWork, TEntity>
    where TDbContext : DbContext
    where TUnitOfWork : EntityFrameworkUnitOfWork<TDbContext>
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity, TDbContext, TUnitOfWork}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected EntityFrameworkRepository(TUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        this.ObjectSet = this.UnitOfWork.Context.Set<TEntity>();
    }

    /// <summary>
    /// Gets the object set.
    /// </summary>
    protected DbSet<TEntity> ObjectSet { get; }

    /// <summary>
    /// Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="ArgumentNullException">Cannot add a null entity - entity.</exception>
    public override void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.ObjectSet.Add(entity);
    }

    /// <summary>
    /// Removes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="ArgumentNullException">Cannot remove a null entity - entity.</exception>
    public override void Remove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.ObjectSet.Remove(entity);
    }

    /// <summary>
    /// Refreshes the specified collection of entities in the repository.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="entities">The entities.</param>
    public override void Refresh(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);
        this.UnitOfWork.Refresh(entities);
    }

    /// <summary>
    /// Refreshes the specified entity in the repository.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="entity">The entity.</param>
    public override void Refresh(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        this.UnitOfWork.Refresh(Enumerable.Repeat(entity, 1));
    }

    /// <summary>
    /// Gets all the entities in the current repository.
    /// </summary>
    /// <value>
    /// A collection of entities.
    /// </value>
    public override IEnumerable<TEntity> GetAll() => this.ObjectSet.OfType<TEntity>();
}