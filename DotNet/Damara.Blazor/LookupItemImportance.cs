// <copyright file="LookupItemImportance.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Blazor;

/// <summary>
/// Enumerates lookup item importance.
/// </summary>
public enum LookupItemImportance
{
    /// <summary>
    /// The none importance.
    /// </summary>
    None = 0,

    /// <summary>
    /// The success importance.
    /// </summary>
    Success = 1,

    /// <summary>
    /// The warning importance.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// The errro importance.
    /// </summary>
    Error = 3,
}