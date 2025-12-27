// <copyright file="SerilogConfiguration.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Exposes properties and methods for setting up Serilog configuration.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Gets the logger configuration.
    /// </summary>
    public static LoggerConfiguration LoggerConfiguration { get; } = new LoggerConfiguration();
}