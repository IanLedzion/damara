// <copyright file="PagedQueryResult.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Paged query result.
/// </summary>
/// <typeparam name="TEntityBase">The type of the entity base.</typeparam>
public class PagedQueryResult<TEntityBase>
    where TEntityBase : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedQueryResult{TEntityBase}"/> class.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="isLastPage">if set to <c>true</c> [is last page].</param>
    public PagedQueryResult(TEntityBase[] entities, bool isLastPage)
    {
        this.Entities = entities;
        this.IsLastPage = isLastPage;
    }

    /// <summary>
    /// Gets the entities.
    /// </summary>
    public TEntityBase[] Entities { get; }

    /// <summary>
    /// Gets a value indicating whether this is the last page.
    /// </summary>
    public bool IsLastPage { get; }
}