// <copyright file="EntityValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides common functionality across entities.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class EntityValidator<TUnitOfWork, TEntity> : ValidatorBase<TUnitOfWork, TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityValidator{TEntity, TUnitOfWork}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected EntityValidator(TUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }
}
