// <copyright file="LookupItemCollection{TEntity,TId}.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Damara.Blazor;

/// <summary>
/// Provides a collection of entity lookup instance.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TId">The identifier type.</typeparam>
/// <seealso cref="Collection{LookupItem{TId}}" />
public class LookupItemCollection<TEntity, TId> : Collection<LookupItem<TId>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LookupItemCollection{TEntity, TId}"/> class.
    /// </summary>
    public LookupItemCollection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LookupItemCollection{TEntity, TId}" /> class.
    /// </summary>
    /// <param name="entities">The entity collection.</param>
    /// <param name="idSelector">The identifier selector.</param>
    /// <param name="nameSelector">The name selector.</param>
    /// <param name="orderBySelector">The order by selector.</param>
    /// <param name="descendingSort">if set to <c>true</c> sort by descending order.</param>
    /// <param name="sortItems">if set to <c>true</c> [sort items].</param>
    /// <param name="groupNameSelector">The group name selector.</param>
    /// <param name="importanceSelector">The importance selector.</param>
    /// <param name="specialItemsAtStart">The special items at start.</param>
    /// <param name="specialItemsAtEnd">The special items at end.</param>
    public LookupItemCollection(
        IEnumerable<TEntity> entities,
        Func<TEntity, TId> idSelector,
        Func<TEntity, string> nameSelector,
        Func<TEntity, object> orderBySelector = null,
        bool descendingSort = false,
        bool sortItems = true,
        Func<TEntity, string> groupNameSelector = null,
        Func<TEntity, LookupItemImportance> importanceSelector = null,
        IEnumerable<LookupItem<TId>> specialItemsAtStart = null,
        IEnumerable<LookupItem<TId>> specialItemsAtEnd = null)
    {
        IEnumerable<TEntity> sortedEntities;

        // Sort items if necessary
        if (sortItems)
        {
            if (descendingSort)
            {
                sortedEntities = orderBySelector == null ? entities.AsEnumerable().OrderByDescending(nameSelector) : entities.AsEnumerable().OrderByDescending(orderBySelector);
            }
            else
            {
                sortedEntities = orderBySelector == null ? entities.AsEnumerable().OrderBy(nameSelector) : entities.AsEnumerable().OrderBy(orderBySelector);
            }
        }
        else
        {
            sortedEntities = entities.AsEnumerable();
        }

        // Add special items
        if (specialItemsAtStart != null)
        {
            foreach (var item in specialItemsAtStart)
            {
                this.Add(item);
            }
        }

        // Populate the collection
        foreach (var entity in sortedEntities)
        {
            var id = idSelector(entity);
            var name = nameSelector(entity);
            var groupName = groupNameSelector?.Invoke(entity);
            var importance = importanceSelector?.Invoke(entity) ?? LookupItemImportance.None;

            var lookupItem = new LookupItem<TId>(id, name, groupName, importance);
            this.Add(lookupItem);
        }

        // Add special items
        if (specialItemsAtEnd != null)
        {
            foreach (var item in specialItemsAtEnd)
            {
                this.Add(item);
            }
        }
    }
}