// <copyright file="ConfigurationHelpers.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;

namespace Damara;

/// <summary>
/// Provides methods and properties to interact with application configuration.
/// </summary>
public static class ConfigurationHelpers
{
    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <param name="connectionStringName">Name of the connection string.</param>
    /// <returns>A connection string, or <c>null</c> if no entry was found to match the connection string name.</returns>
    public static string GetConnectionString(string connectionStringName)
    {
        string connectionString;
        var jsonConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        connectionString = jsonConfig.GetConnectionString(connectionStringName);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        return null;
    }
}