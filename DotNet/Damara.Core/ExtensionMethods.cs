// <copyright file="ExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides extension methods.
/// </summary>
public static class ExtensionMethods
{
#nullable enable
    /// <summary>
    /// Returns ancestors for the specified type.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="e">The object instance.</param>
    /// <param name="f">A Function returning the property to walk to find the ancestors.</param>
    /// <returns>An enumerable collection of ancestor types.</returns>
    public static IEnumerable<T> Ancestors<T>(this T e, Func<T, T?> f)
    {
        if (e == null)
        {
            return null!;
        }

        var ancestors = new List<T>() { e };
        var parent = f(e);

        if (parent != null)
        {
            ancestors.AddRange(parent.Ancestors(f));
        }

        return ancestors;
    }
#nullable disable

    /// <summary>
    /// Flattens a tree.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="e">The e to act on.</param>
    /// <param name="f">A Function returning the property to walk to find the ancestors.</param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process flatten in this collection.
    /// </returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> e, Func<T, IEnumerable<T>> f)
        => e.SelectMany(c => f(c).Flatten(f)).Concat(e);
}