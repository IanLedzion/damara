// <copyright file="EntityValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation.Results;

namespace Damara;

/// <summary>
/// Provides common functionality across entities.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class EntityValidator<TUnitOfWork, TEntity> : AbstractValidator<TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityValidator{TEntity, TUnitOfWork}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected EntityValidator(TUnitOfWork unitOfWork)
    {
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Determines if validation should occur and provides a means to modify the context and ValidationResult prior to execution.
    /// If this method returns false, then the ValidationResult is immediately returned from Validate/ValidateAsync.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="result">The validation result.</param>
    /// <returns>A value indicating whether validation should continue.</returns>
    protected override bool PreValidate(ValidationContext<TEntity> context, ValidationResult result)
    {
        this.UnitOfWork.OnBeforeValidate();
        return true;
    }
}