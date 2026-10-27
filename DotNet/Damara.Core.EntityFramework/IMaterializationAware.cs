// <copyright file="IMaterializationAware.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.EntityFramework;

/// <summary>
/// Implemented by entities that need to act once EF Core has loaded them from the database,
/// in place of LinqConnect's <c>OnLoaded</c> partial method. <see cref="EntityFrameworkUnitOfWork{TDbContext}"/>
/// makes the call when its context starts tracking an entity a query returned.
/// </summary>
public interface IMaterializationAware
{
    /// <summary>
    /// Called once a tracking query has materialized the entity and the context tracks it, before it is
    /// returned to the caller. Navigations may be lazy loaded here.
    /// </summary>
    void OnMaterialized();
}
