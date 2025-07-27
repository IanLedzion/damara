// <copyright file="SubscribingPageModelValidator[TUnitOfWork, TEntityBase, TPageModel].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Damara.Blazor;

/// <summary>
/// Provides a base class for page model validation.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Generic type")]
public abstract class SubscribingPageModelValidator<TUnitOfWork, TEntityBase, TPageModel> : AbstractValidator<TPageModel>
    where TUnitOfWork : IUnitOfWork
    where TEntityBase : EntityBase
    where TPageModel : SubscribingPageModelBase<TUnitOfWork, TEntityBase>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribingPageModelValidator{TUnitOfWork, TEntityBase, TPageModel}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected SubscribingPageModelValidator(TUnitOfWork unitOfWork)
    {
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }
}