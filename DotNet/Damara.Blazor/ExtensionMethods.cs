// <copyright file="ExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace Damara.Blazor;

/// <summary>
/// Provides extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Converts the entities to a lookup item collection.
    /// </summary>
    /// <param name="entities">The entity collection.</param>
    /// <param name="idSelector">The identifier selector.</param>
    /// <param name="nameSelector">The name selector.</param>
    /// <param name="orderBySelector">The order by selector.</param>
    /// <param name="descendingSort">if set to <c>true</c> sort by descending order.</param>
    /// <param name="groupNameSelector">The group name selector.</param>
    /// <param name="specialItemsAtStart">The special items at start.</param>
    /// <param name="specialItemsAtEnd">The special items at end.</param>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The identifier type.</typeparam>
    public static LookupItemCollection<TEntity, TId> ToLookupItemCollection<TEntity, TId>(
        this IEnumerable<TEntity> entities,
        Func<TEntity, TId> idSelector,
        Func<TEntity, string> nameSelector,
        Func<TEntity, object> orderBySelector = null,
        bool descendingSort = false,
        Func<TEntity, string> groupNameSelector = null,
        Func<TEntity, LookupItemImportance> importanceSelector = null,
        IEnumerable<LookupItem<TId>> specialItemsAtStart = null,
        IEnumerable<LookupItem<TId>> specialItemsAtEnd = null)
        => new(
            entities: entities,
            idSelector: idSelector,
            nameSelector: nameSelector,
            orderBySelector: orderBySelector,
            descendingSort: descendingSort,
            groupNameSelector: groupNameSelector,
            importanceSelector: importanceSelector,
            specialItemsAtStart: specialItemsAtStart,
            specialItemsAtEnd: specialItemsAtEnd);
}