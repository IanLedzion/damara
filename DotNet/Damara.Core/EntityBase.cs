// <copyright file="EntityBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides a base class from which all entities are derived.
/// </summary>
public abstract class EntityBase
{
    private static long createSequence;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase"/> class.
    /// </summary>
    protected EntityBase()
    {
        this.CreateSequence = createSequence++;
    }

    /// <summary>
    /// Gets the create sequence.
    /// </summary>
    public long CreateSequence { get; }
}