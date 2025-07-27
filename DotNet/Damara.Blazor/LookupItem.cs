// <copyright file="LookupItem.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Blazor;

/// <summary>
/// Provides a wrapper around an entity as a lookup.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public class LookupItem<TId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LookupItem{TId}"/> class.
    /// </summary>
    public LookupItem()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LookupItem{TId}" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="groupName">Name of the group.</param>
    public LookupItem(TId id, string name, string groupName = null, LookupItemImportance importance = LookupItemImportance.None)
    {
        this.Id = id;
        this.Name = name;
        this.GroupName = groupName ?? string.Empty;
        this.Importance = importance;
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the grouping name.
    /// </summary>
    public string GroupName { get; }

    /// <summary>
    /// Gets the importance.
    /// </summary>
    public LookupItemImportance Importance { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.Name;
    }
}