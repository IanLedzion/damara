// <copyright file="IRepository.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Interface for repositories.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepository<TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// Adds the specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Add(TEntity entity);

    /// <summary>
    /// Removes the specified entity from the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Remove(TEntity entity);

    /// <summary>
    /// Refreshes the specified collection of entities in the repository.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="entities">The entities.</param>
    public void Refresh(IEnumerable<TEntity> entities);

    /// <summary>
    /// Refreshes the specified entity in the repository.
    /// </summary>
    /// <param name="entity">The entity.</param>
    public void Refresh(TEntity entity);

    /// <summary>
    /// Gets all the entities in the current repository.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    public IEnumerable<TEntity> GetAll();
}