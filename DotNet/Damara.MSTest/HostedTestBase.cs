// <copyright file="HostedTestBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Damara.MSTest;

/// <summary>
/// Base class for tests in a DI environment.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
public abstract class HostedTestBase<TUnitOfWork>
    where TUnitOfWork : UnitOfWorkBase
{
    private readonly Lazy<IServiceProvider> scopedServiceProviderFactory;
    private readonly Lazy<TUnitOfWork> unitOfWorkFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="HostedTestBase{TUnitOfWork}" /> class.
    /// </summary>
    protected HostedTestBase()
    {
        this.Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
          .ConfigureHostConfiguration(this.ConfigureHostConfiguration)
          .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
          .ConfigureServices(this.ConfigureServices)
          .Build();

        this.scopedServiceProviderFactory = new Lazy<IServiceProvider>(() =>
        this.Host
        .Services
        .CreateScope()
        .ServiceProvider);

        this.unitOfWorkFactory = new Lazy<TUnitOfWork>(this.Services.GetRequiredService<TUnitOfWork>);
    }

    /// <summary>
    /// Gets the host.
    /// </summary>
    public IHost Host { get; }

    /// <summary>
    /// Gets the services.
    /// </summary>
    protected IServiceProvider Services => this.scopedServiceProviderFactory.Value;

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork => this.unitOfWorkFactory.Value;

    /// <summary>
    /// Configures the host configuration.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder.</param>
    protected virtual void ConfigureHostConfiguration(IConfigurationBuilder configurationBuilder)
    {
    }

    /// <summary>
    /// Configures the application configuration.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="configurationBuilder">The configuration builder.</param>
    protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            configurationBuilder.AddUserSecrets(this.GetType().Assembly);
        }
    }

    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="serviceCollection">The service collection.</param>
    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
    {
    }
}
