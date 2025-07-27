// <copyright file="DmlCommandType.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Logging;

/// <summary>
/// Represents a DML command type.
/// </summary>
[Flags]
public enum DmlCommandType
{
    /// <summary>
    /// The select command type.
    /// </summary>
    Select = 1,

    /// <summary>
    /// The insert command type.
    /// </summary>
    Insert = 2,

    /// <summary>
    /// The update command type.
    /// </summary>
    Update = 4,

    /// <summary>
    /// The delete command type.
    /// </summary>
    Delete = 8,

    /// <summary>
    /// The execute command type.
    /// </summary>
    Exec = 16,
}