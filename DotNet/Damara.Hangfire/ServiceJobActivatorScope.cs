// <copyright file="ServiceJobActivatorScope.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Damara.Hangfire;

/// <summary>
/// A job activator scope.
/// </summary>
public class ServiceJobActivatorScope : JobActivatorScope
{
    private readonly IServiceScope serviceScope;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceJobActivatorScope"/> class.
    /// </summary>
    /// <param name="serviceScope">The service scope.</param>
    /// <exception cref="ArgumentNullException">serviceScope.</exception>
    public ServiceJobActivatorScope(IServiceScope serviceScope)
    {
        if (serviceScope == null)
        {
            throw new ArgumentNullException(nameof(serviceScope));
        }

        this.serviceScope = serviceScope;
    }

    /// <summary>
    /// Resolves the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A service instance.</returns>
    public override object Resolve(Type type)
    {
        return this.serviceScope.ServiceProvider.GetService(type);
    }
}