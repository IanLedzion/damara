// <copyright file="QueryResult.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Encapsulates the result of a query.
/// </summary>
public class QueryResult<TEntityBase>
    where TEntityBase : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryResult{TEntityBase}"/> class.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="totalCount">Number of totals.</param>
    public QueryResult(TEntityBase[] entities, int totalCount)
    {
        this.Entities = entities;
        this.TotalCount = totalCount;
    }

    /// <summary>
    /// Gets the entities.
    /// </summary>
    public TEntityBase[] Entities { get; }

    /// <summary>
    /// Gets the total number of entities.
    /// </summary>
    public int TotalCount { get; }
}