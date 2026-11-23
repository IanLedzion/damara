// <copyright file="PhoneValidatorTests.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Core.Tests;

/// <summary>
/// Provides tests for the phone number validator (<see cref="ValidationExtensionMethods.PhoneNumber{T}(IRuleBuilder{T, string}, string)"/>).
/// </summary>
[TestClass]
public class PhoneValidatorTests
{
    /// <summary>
    /// Tests that mobile and landline numbers, Swiss and foreign, national and international, with the usual
    /// separators, are valid.
    /// </summary>
    /// <param name="phone">The phone number.</param>
    [TestMethod]
    [DataRow("079 123 45 67")]
    [DataRow("0791234567")]
    [DataRow("+41 79 123 45 67")]
    [DataRow("0041 79 123 45 67")]
    [DataRow("061 123 45 67")]
    [DataRow("0611234567")]
    [DataRow("+41 61 123 45 67")]
    [DataRow("0041 61 123 45 67")]
    [DataRow("061/123 45 67")]
    [DataRow("061-123-45-67")]
    [DataRow("061.123.45.67")]
    [DataRow("(061) 123 45 67")]
    [DataRow("+49 30 1234567")]
    [DataRow("0049 30 1234567")]
    [DataRow("+33 1 23 45 67 89")]
    [DataRow("0033 1 23 45 67 89")]
    [DataRow("+39 02 1234 5678")]
    [DataRow("+44 20 7946 0958")]
    public void PhoneNumber_ValidNumber_Valid(string phone)
    {
        var result = Validate(phone);

        Assert.IsTrue(result.IsValid, $"'{phone}' rejected");
    }

    /// <summary>
    /// Tests that malformed numbers are invalid.
    /// </summary>
    /// <param name="phone">The phone number.</param>
    [TestMethod]
    [DataRow("061 123 45")]
    [DataRow("12345")]
    [DataRow("061 123 45 67 x")]
    [DataRow("061_123_45_67")]
    [DataRow("+41 61 123 45 67 89 01 23")]
    [DataRow("0041 6112345678901234")]
    public void PhoneNumber_InvalidNumber_Invalid(string phone)
    {
        var result = Validate(phone);

        Assert.IsFalse(result.IsValid, $"'{phone}' accepted");
    }

    /// <summary>
    /// Validates a phone number with the Swiss default region.
    /// </summary>
    /// <param name="phone">The phone number.</param>
    /// <returns>The validation result.</returns>
    private static ValidationResult Validate(string phone)
    {
        var validator = new InlineValidator<PhoneHolder>();
        validator.RuleFor(holder => holder.Phone).PhoneNumber("CH");

        return validator.Validate(new PhoneHolder { Phone = phone });
    }

    /// <summary>
    /// Holds a phone number to validate.
    /// </summary>
    private sealed class PhoneHolder
    {
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string Phone { get; set; }
    }
}
