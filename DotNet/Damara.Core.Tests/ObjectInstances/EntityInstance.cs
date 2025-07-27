// <copyright file="EntityInstance.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Entity instance.
/// </summary>
/// <seealso cref="Damara.EntityBase" />
public class EntityInstance : EntityBase
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    public string Status { get; set; }
}
