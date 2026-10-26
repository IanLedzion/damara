// <copyright file="MaterializationAwareInterceptor.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Damara.EntityFramework;

/// <summary>
/// Calls <see cref="IMaterializationAware.OnMaterialized"/> on every entity EF Core materializes
/// that implements it. Register it in the database context's options, for example
/// <c>optionsBuilder.AddInterceptors(MaterializationAwareInterceptor.Instance)</c>.
/// </summary>
public sealed class MaterializationAwareInterceptor : IMaterializationInterceptor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MaterializationAwareInterceptor"/> class.
    /// </summary>
    private MaterializationAwareInterceptor()
    {
    }

    /// <summary>
    /// Gets the shared instance. The interceptor holds no state, so one serves every context.
    /// </summary>
    public static MaterializationAwareInterceptor Instance { get; } = new();

    /// <summary>
    /// Called once the entity instance has been created and its properties set from the query.
    /// </summary>
    /// <param name="materializationData">Contextual information about the materialization.</param>
    /// <param name="entity">The entity instance that has been materialized.</param>
    /// <returns>The entity instance, unchanged.</returns>
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if (entity is IMaterializationAware materializationAware)
        {
            materializationAware.OnMaterialized();
        }

        return entity;
    }
}
