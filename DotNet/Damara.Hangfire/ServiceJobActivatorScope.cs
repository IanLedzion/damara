// <copyright file="ServiceJobActivatorScope.cs" company="Ian Ledzion.">
// Copyright (c) Ian Ledzion. All rights reserved.
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

    /// <summary>
    /// Disposes the service scope once Hangfire has finished performing the job.
    /// </summary>
    /// <remarks>
    /// The base implementation is empty, so without this override the per-job scope, and every scoped service in it, outlived
    /// the job. In NsaGarantie that kept each job's unit of work registered in the static <c>UnitOfWorkDescriptor</c> registry
    /// for the life of the process, which grew the background job server's heap to its limit every two to three days.
    /// </remarks>
    public override void DisposeScope()
    {
        this.serviceScope.Dispose();
    }
}