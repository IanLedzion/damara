// <copyright file="ExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Reflection;
using Damara.DependencyInjection;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Damara.Hangfire;

/// <summary>
/// Provides extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Registers Damara hangfire services.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="assembly">The assembly.</param>
    /// <returns>
    /// The <see cref="IServiceCollection" /> instance for method chaining.
    /// </returns>
    /// <exception cref="InvalidOperationException">Could not load service type.</exception>
    public static IServiceCollection AddDamaraHangfireServices(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var serviceType = typeof(HangfireServiceBase<>);

        var types =
            assembly
            .GetTypes()
            .Where(type => type.IsSubclassOfGeneric(serviceType) && !type.IsAbstract);

        foreach (var type in types)
        {
            try
            {
                var serviceInterfaces = type.GetInterfaces().Where(i => i.GetCustomAttribute<HangfireServiceAttribute>() != null);

                foreach (var serviceInteface in serviceInterfaces)
                {
                    serviceCollection.AddScoped(serviceInteface, type);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Could not load service type \"{0}\", an exception of type {1} was thrown.", type.FullName, ex.GetType().FullName),
                    ex);
            }
        }

        return serviceCollection;
    }

    /// <summary>
    /// Enqueues A job or continues a job.
    /// </summary>
    /// <typeparam name="T">The service type.</typeparam>
    /// <param name="backgroundJobClient">The background job client.</param>
    /// <param name="jobId">The job identifier.</param>
    /// <param name="methodCall">The method call.</param>
    /// <returns>A job id.</returns>
    public static string EnqueueOrContinueJobWith<T>(this IBackgroundJobClient backgroundJobClient, string jobId, System.Linq.Expressions.Expression<Action<T>> methodCall)
    {
        if (string.IsNullOrWhiteSpace(jobId))
        {
            return backgroundJobClient.Enqueue(methodCall);
        }
        else
        {
            return backgroundJobClient.ContinueJobWith(jobId, methodCall);
        }
    }
}