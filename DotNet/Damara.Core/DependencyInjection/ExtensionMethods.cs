// <copyright file="ExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Damara.DependencyInjection;

/// <summary>
/// Provides extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Registers Damara application services.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="assemblies">The assemblies.</param>
    /// <returns>
    /// The <see cref="IServiceCollection" /> instance for method chaining.
    /// </returns>
    /// <exception cref="InvalidOperationException">Could not load service type.</exception>
    public static IServiceCollection AddDamaraApplicationServices(this IServiceCollection serviceCollection, params Assembly[] assemblies)
    {
        foreach (var assy in assemblies)
        {
            var interfaces =
                assy
                .GetTypes()
                .Where(type => type.IsInterface && type.GetCustomAttribute<ApplicationServiceAttribute>() != null);

            var services =
                assy
                .GetTypes()
                .Where(type => !type.IsAbstract && type.GetInterfaces().Any(i => interfaces.Contains(i)));

            var serviceType = typeof(ApplicationServiceBase<>);

            var types =
                assy
                .GetTypes()
                .Where(type => type.IsSubclassOfGeneric(serviceType) && !type.IsAbstract);

            foreach (var @interface in interfaces)
            {
                foreach (var service in services.Where(service => service.GetInterfaces().Any(i => i == @interface)))
                {
                    var key = service.GetCustomAttribute<ServiceKeyAttribute>()?.Key;
                    if (key == null)
                    {
                        serviceCollection.AddScoped(@interface, service);
                    }
                    else
                    {
                        serviceCollection.AddKeyedScoped(@interface, key, service);
                    }
                }
            }
        }

        return serviceCollection;
    }

    /// <summary>
    /// Determines whether the class is a subclass of a generic generic base class.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="baseClass">The base class.</param>
    /// <returns>
    ///   <c>true</c> if the class is a subclass of a generic generic base class; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsSubclassOfGeneric(this Type type, Type baseClass)
    {
        while (type != null && type != typeof(object))
        {
            var current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (baseClass == current)
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;
    }
}