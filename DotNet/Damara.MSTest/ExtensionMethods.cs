// <copyright file="ExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation.Results;

namespace Damara.MSTest;

/// <summary>
/// Provides extension methods.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// A ValidationResult extension method that queries if a validation result has failures for the specified property.
    /// </summary>
    /// <param name="validationResult">The validationResult to act on.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>
    /// True if errors for property, false if not.
    /// </returns>
    public static bool HasFailuresForProperty(this ValidationResult validationResult, string propertyName)
    {
        return validationResult.Errors.Any(failure => failure.PropertyName == propertyName);
    }

    /// <summary>
    /// A ValidationResult extension method that returns the number of failures for the specified property.
    /// </summary>
    /// <param name="validationResult">The validationResult to act on.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>
    /// An int.
    /// </returns>
    public static int PropertyFailurCount(this ValidationResult validationResult, string propertyName)
    {
        return validationResult.Errors.Count(failure => failure.PropertyName == propertyName);
    }
}