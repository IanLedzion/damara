// <copyright file="LoggableBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Reflection;
using Serilog;

namespace Damara;

/// <summary>
/// Base class which exposes a Logger.
/// </summary>
public abstract class LoggableBase
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggableBase" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    protected LoggableBase(ILogger logger)
    {
        var random = new Random();
        var instanceId = new string(
            Enumerable.Repeat(Chars, 10)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());

        var type = this.GetType();

        var loggerInstance = logger ?? new LoggerConfiguration().CreateLogger();

        this.Logger =
            loggerInstance
            .ForContext(type)
            .ForContext("InstanceId", instanceId);

        var assyInfoVersion = type.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        if (!string.IsNullOrWhiteSpace(assyInfoVersion))
        {
            this.Logger = this.Logger.ForContext("AssemblyVersion", assyInfoVersion);
        }
    }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger Logger { get; }
}