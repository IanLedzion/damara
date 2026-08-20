// <copyright file="ValidatorBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation.Results;

namespace Damara;

/// <summary>
/// Provides common functionality for validators that carry the unit of work.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="T">The type being validated.</typeparam>
public abstract class ValidatorBase<TUnitOfWork, T> : AbstractValidator<T>
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatorBase{TUnitOfWork, T}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected ValidatorBase(TUnitOfWork unitOfWork)
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
    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        this.UnitOfWork.OnBeforeValidate();
        return true;
    }
}
