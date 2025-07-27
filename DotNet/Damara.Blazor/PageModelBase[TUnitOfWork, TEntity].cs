// <copyright file="PageModelBase[TUnitOfWork, TEntity].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Serilog;

namespace Damara.Blazor;

/// <summary>
/// Base class for read-only page models.
/// </summary>
/// <typeparam name="TUnitOfWork">Type of the unit of work.</typeparam>
/// <typeparam name="TEntityBase">Type of the entity base.</typeparam>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Generic type")]
public abstract class PageModelBase<TUnitOfWork, TEntityBase> : PageModelBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
    where TEntityBase : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageModelBase{TUnitOfWork, TEntityBase}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="entity">The entity.</param>
    protected PageModelBase(TUnitOfWork unitOfWork, ILogger logger, TEntityBase? entity)
        : base(unitOfWork, logger)
    {
        this.Entity = entity;
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    [JsonProperty]
    public TEntityBase Entity { get; }
}