// <copyright file="UrlValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation;
using FluentValidation.Validators;

namespace Damara.Validation;

/// <summary>
/// Mail address validator using System.Net.Mail.
/// </summary>
/// <typeparam name="T">Generic type parameter.</typeparam>
internal class UrlValidator<T> : PropertyValidator<T, string>
{
    private const string AllowedCharacters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;=";

    /// <summary>
    /// Gets the name.
    /// </summary>
    public override string Name => "Url";

    /// <summary>
    /// Validates the value..
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="value">The value.</param>
    /// <returns>
    /// True if valid, false if not.
    /// </returns>
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        foreach (var c in value)
        {
            if (!AllowedCharacters.Contains(c))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns the default error message template for this validator, when not overridden.
    /// </summary>
    /// <param name="errorCode">The currently configured error code for the validator.</param>
    /// <returns>
    /// The default message template.
    /// </returns>
    protected override string GetDefaultMessageTemplate(string errorCode)
      => "'{PropertyName}' is not a valid URL.";
}