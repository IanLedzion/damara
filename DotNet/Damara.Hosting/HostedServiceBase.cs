// <copyright file="HostedServiceBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.Extensions.Hosting;
using Serilog;

namespace Damara.Hosting;

/// <summary>
/// Base class for hosted services.
/// </summary>
/// <seealso cref="IHostedService" />
public abstract class HostedServiceBase<TUnitOfWork> : IHostedService
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HostedServiceBase{TUnitOfWork}"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="hostApplicationLifetime">The host application lifetime.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    protected HostedServiceBase(ILogger logger, IHostApplicationLifetime hostApplicationLifetime, TUnitOfWork unitOfWork)
    {
        this.Logger = logger;
        this.HostApplicationLifetime = hostApplicationLifetime;
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the host application lifetime.
    /// </summary>
    protected IHostApplicationLifetime HostApplicationLifetime { get; }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A public Task.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.HostApplicationLifetime.ApplicationStarted.Register(async () =>
        {
            await this.ExecuteAsync();
            this.Execute();
            this.HostApplicationLifetime.StopApplication();
        });

        return Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    /// <returns>A public Task.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Executes this instance.
    /// </summary>
    protected virtual void Execute()
    {
    }

    /// <summary>
    /// Executes this instance asynchronously.
    /// </summary>
    /// <returns>A public Task.</returns>
    protected virtual Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}