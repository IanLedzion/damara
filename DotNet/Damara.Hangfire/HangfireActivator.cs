// <copyright file="HangfireActivator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Damara.Hangfire;

/// <summary>
/// Hangfire activator utility.
/// </summary>
public class HangfireActivator : JobActivator
{
    /// <summary>
    /// (Immutable) the service provider.
    /// </summary>
    private readonly IServiceProvider serviceProvider;
    private readonly IServiceScopeFactory serviceScopeFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireActivator"/> class.
    /// </summary>
    /// <param name="serviceProvider">(Immutable) the service provider.</param>
    public HangfireActivator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
    }

    /// <summary>
    /// Begins the scope.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The job activator scope.</returns>
    public override JobActivatorScope BeginScope(JobActivatorContext context)
    {
        return new ServiceJobActivatorScope(this.serviceScopeFactory.CreateScope());
    }

    /// <summary>
    /// Begins the scope.
    /// </summary>
    /// <returns>The job activator scope.</returns>
    [Obsolete]
    public override JobActivatorScope BeginScope()
    {
        return new ServiceJobActivatorScope(this.serviceScopeFactory.CreateScope());
    }

    /// <summary>
    /// Begins the scope.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The job activator scope.</returns>
    public override JobActivatorScope BeginScope(PerformContext context)
    {
        return new ServiceJobActivatorScope(this.serviceScopeFactory.CreateScope());
    }

    /// <summary>
    /// Activates the job.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// An object.
    /// </returns>
    public override object ActivateJob(Type type)
    {
        return this.serviceProvider.GetRequiredService(type);
    }
}