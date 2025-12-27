// <copyright file="PhoneValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using PhoneNumbers;

namespace Damara.Validation;

/// <summary>
/// Mail address validator using System.Net.Mail.
/// </summary>
/// <typeparam name="T">Generic type parameter.</typeparam>
internal class PhoneValidator<T> : PropertyValidator<T, string>
{
    private const string AllowedCharacters = @"+01234567890 ()";
    private readonly Func<T, string> defaultRegion;

    /// <summary>
    /// Initializes a new instance of the <see cref="PhoneValidator{T}" /> class.
    /// </summary>
    /// <param name="defaultRegion">The default region.</param>
    public PhoneValidator(Func<T, string> defaultRegion)
    {
        this.defaultRegion = defaultRegion;
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public override string Name => "Phone";

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

        if (value.Length > 15)
        {
            return false;
        }

        foreach (var c in value)
        {
            if (!AllowedCharacters.Contains(c))
            {
                return false;
            }
        }

        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var phoneNumber = phoneNumberUtil.Parse(value, this.defaultRegion.Invoke(context.InstanceToValidate));

            if (!phoneNumberUtil.IsValidNumber(phoneNumber))
            {
                return false;
            }
        }
        catch (NumberParseException)
        {
            return false;
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
      => "'{PropertyName}' is not a valid phone number.";
}