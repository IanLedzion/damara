// <copyright file="LookupItemCollectionFactory.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Damara.Blazor;

/// <summary>
/// Lookup item collection.
/// </summary>
public static class LookupItemCollectionFactory
{
    /// <summary>
    /// Creates a boolean false-true collection.
    /// </summary>
    /// <param name="falseName">Name of the false value.</param>
    /// <param name="trueName">Name of the true value.</param>
    /// <returns>
    /// The new bool false true collection.
    /// </returns>
    public static LookupItemCollection<string, bool> CreateBoolFalseTrueCollection(string falseName, string trueName)
    {
        return
        [
            new LookupItem<bool>(false, falseName),
            new LookupItem<bool>(true, trueName),
        ];
    }

    /// <summary>
    /// Creates a boolean true-false collection.
    /// </summary>
    /// <param name="trueName">Name of the true value.</param>
    /// <param name="falseName">Name of the false value.</param>
    /// <returns>
    /// The new bool false true collection.
    /// </returns>
    public static LookupItemCollection<string, bool> CreateBoolTrueFalseCollection(string trueName, string falseName)
    {
        return
        [
            new LookupItem<bool>(true, trueName),
            new LookupItem<bool>(false, falseName),
        ];
    }

    /// <summary>
    /// Creates a collection from an enumeration.
    /// </summary>
    /// <typeparam name="TEnum">Type of the enum.</typeparam>
    /// <returns>
    /// The new collection.
    /// </returns>
    public static LookupItemCollection<string, TEnum> CreateEnumCollection<TEnum>()
                    where TEnum : Enum
    {
        var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>();
        var names = Enum.GetNames(typeof(TEnum));

        var collection = new LookupItemCollection<string, TEnum>();

        for (var i = 0; i < values.Count(); i++)
        {
            var lookupItem = new LookupItem<TEnum>(values.ElementAt(i), Regex.Replace(names[i], "[A-Z]", " $&").Trim());
            collection.Add(lookupItem);
        }

        return collection;
    }

    /// <summary>
    /// Creates a collection from a dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary.</param>
    /// <returns>A collection.</returns>
    public static LookupItemCollection<string, TId> CreateDictionaryCollection<TId>(IDictionary<TId, string> dictionary)
    {
        var collection = new LookupItemCollection<string, TId>();
        foreach (var item in dictionary)
        {
            collection.Add(new LookupItem<TId>(item.Key, item.Value));
        }

        return collection;
    }
}