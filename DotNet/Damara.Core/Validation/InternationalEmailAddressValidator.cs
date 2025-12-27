// <copyright file="InternationalEmailAddressValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using EmailValidation;

namespace Damara.Validation;

/// <summary>
///  Mail address validator using the <see cref="EmailValidator"> library.
/// </summary>
/// <typeparam name="T">The type parameter.</typeparam>
internal class InternationalEmailAddressValidator<T> : PropertyValidator<T, string>
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <inheritdoc />
    public override string Name => "EmailAddress";

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
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        return EmailValidator.Validate(value);
    }

    /// <summary>
    /// Returns the default error message template for this validator, when not overridden.
    /// </summary>
    /// <param name="errorCode">The currently configured error code for the validator.</param>
    /// <returns>
    /// The default message template.
    /// </returns>
    protected override string GetDefaultMessageTemplate(string errorCode)
      => "'{PropertyName}' is not a valid email address.";
}