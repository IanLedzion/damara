//-----------------------------------------------------------------------
// <copyright file="ValidationTarget.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Provides a target for validation tests.
/// </summary>
public class ValidationTarget
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationTarget"/> class.
    /// </summary>
    public ValidationTarget()
    {
        this.Name = string.Empty;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }
}
