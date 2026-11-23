// <copyright file="PhoneValidator.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using PhoneNumbers;

namespace Damara.Validation;

/// <summary>
/// Phone number validator using libphonenumber: accepts any valid number (mobile or landline) of any country,
/// in national format for the default region or in international format (+ or the region's international prefix).
/// </summary>
/// <typeparam name="T">Generic type parameter.</typeparam>
internal class PhoneValidator<T> : PropertyValidator<T, string>
{
    /// <summary>
    /// The characters allowed besides digits: the leading plus and the usual separators (space, brackets, slash,
    /// hyphen, dot), as in "+41 61 123 45 67", "061/123 45 67" or "(061) 123-45-67".
    /// </summary>
    private const string AllowedSeparators = @"+ ()/-.";

    /// <summary>
    /// The maximum number of digits in a phone number (E.164), separators not counted.
    /// </summary>
    private const int MaximumDigits = 15;

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

        if (value.Any(c => !char.IsAsciiDigit(c) && !AllowedSeparators.Contains(c)))
        {
            return false;
        }

        // An international prefix "00" is two digits more than the "+" it stands for.
        var digits = value.Count(char.IsAsciiDigit);
        var maximumDigits = value.TrimStart().StartsWith("00", StringComparison.Ordinal) ? MaximumDigits + 2 : MaximumDigits;
        if (digits > maximumDigits)
        {
            return false;
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