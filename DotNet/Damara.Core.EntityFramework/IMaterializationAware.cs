// <copyright file="IMaterializationAware.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.EntityFramework;

/// <summary>
/// Implemented by entities that need to act once EF Core has loaded them from the database,
/// in place of LinqConnect's <c>OnLoaded</c> partial method. The call is made by
/// <see cref="MaterializationAwareInterceptor"/>, which the database context must register.
/// </summary>
public interface IMaterializationAware
{
    /// <summary>
    /// Called after the entity has been materialized from a query and its properties set,
    /// before it is returned to the caller.
    /// </summary>
    void OnMaterialized();
}
