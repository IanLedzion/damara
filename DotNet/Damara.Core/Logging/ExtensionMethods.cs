// <copyright file="ExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Serilog.Events;

namespace Damara.Logging;

/// <summary>
/// Logging extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Returns a value indicating whether the log event is of a command type.
    /// </summary>
    /// <param name="logEvent">The log event.</param>
    /// <param name="commandTypes">The command types.</param>
    /// <returns>True if the log event is of a command type.</returns>
    public static bool OfCommandType(this LogEvent logEvent, DmlCommandType commandTypes)
    {
        if (!logEvent.Properties.TryGetValue(SqlCommandWriter.DmlCommandType, out var commandTypeValue))
        {
            return false;
        }

        var commandType = commandTypeValue.ToString().Replace("\"", string.Empty);

        if (commandTypes.HasFlag(DmlCommandType.Select))
        {
            return commandType == "SELECT";
        }
        else if (commandTypes.HasFlag(DmlCommandType.Insert))
        {
            return commandType == "INSERT";
        }
        else if (commandTypes.HasFlag(DmlCommandType.Update))
        {
            return commandType == "UPDATE";
        }
        else if (commandTypes.HasFlag(DmlCommandType.Delete))
        {
            return commandType == "DELETE";
        }
        else if (commandTypes.HasFlag(DmlCommandType.Exec))
        {
            return commandType == "EXEC";
        }

        return false;
    }
}