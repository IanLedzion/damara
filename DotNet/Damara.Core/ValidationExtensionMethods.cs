// <copyright file="ValidationExtensionMethods.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Damara.Validation;

namespace Damara;

/// <summary>
/// Validation extension methods.
/// </summary>
public static class ValidationExtensionMethods
{
    /// <summary>
    /// Validates that the supplied string is a valid email address.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="ruleBuilder">The ruleBuilder to act on.</param>
    /// <returns>
    /// An IRuleBuilderOptions&lt;T,string&gt;.
    /// </returns>
    public static IRuleBuilderOptions<T, string> InternationalEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new InternationalEmailAddressValidator<T>());
    }

    /// <summary>
    /// Validates that the supplied string is a valid phone number.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="ruleBuilder">The ruleBuilder to act on.</param>
    /// <param name="defaultRegion">The default region.</param>
    /// <returns>
    /// An IRuleBuilderOptions&lt;T,string&gt;.
    /// </returns>
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder, string defaultRegion)
    {
        return ruleBuilder.SetValidator(new PhoneValidator<T>(instance => defaultRegion));
    }

    /// <summary>
    /// Validates that the supplied string is a valid phone number.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="ruleBuilder">The ruleBuilder to act on.</param>
    /// <param name="defaultRegion">The default region.</param>
    /// <returns>
    /// An IRuleBuilderOptions&lt;T,string&gt;.
    /// </returns>
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder, Func<T, string> defaultRegion)
    {
        return ruleBuilder.SetValidator(new PhoneValidator<T>(defaultRegion));
    }

    /// <summary>
    /// Validates that the supplied string is a valid URL.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="ruleBuilder">The ruleBuilder to act on.</param>
    /// <returns>
    /// An IRuleBuilderOptions&lt;T,string&gt;.
    /// </returns>
    public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new UrlValidator<T>());
    }
}