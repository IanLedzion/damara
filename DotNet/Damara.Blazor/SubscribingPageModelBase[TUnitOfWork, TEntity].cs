// <copyright file="SubscribingPageModelBase[TUnitOfWork, TEntity].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Serilog;

namespace Damara.Blazor;

/// <summary>
/// Provides a base page model class which scubscribes to data layer events.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Generic type")]
public abstract class SubscribingPageModelBase<TUnitOfWork, TEntity> : SubscribingPageModelBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribingPageModelBase{TUnitOfWork, TEntity}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="entity">The entity.</param>
    protected SubscribingPageModelBase(TUnitOfWork unitOfWork, ILogger logger, TEntity entity)
        : base(unitOfWork, logger)
    {
        this.Entity = entity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribingPageModelBase{TUnitOfWork, TEntity}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="isNew">if set to <c>true</c> [is new].</param>
    protected SubscribingPageModelBase(TUnitOfWork unitOfWork, ILogger logger, TEntity entity, bool isNew)
        : base(unitOfWork, logger, isNew)
    {
        this.Entity = entity;
    }

    /// <summary>
    /// Gets the entity.
    /// </summary>
    [JsonProperty]
    public TEntity Entity { get; }
}