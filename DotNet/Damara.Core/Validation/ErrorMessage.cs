// <copyright file="ErrorMessage.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Validation;

/// <summary>
/// Represents an error message.
/// </summary>
public class ErrorMessage
{
    /// <summary>
    /// Gets the severity.
    /// </summary>
    [JsonProperty]
    public string Severity { get; private set; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    [JsonProperty]
    public string Message { get; private set; }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return $"[{this.Severity}] {this.Message}";
    }
}