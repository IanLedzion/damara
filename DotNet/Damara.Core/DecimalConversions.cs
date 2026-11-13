// <copyright file="DecimalConversions.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Globalization;

namespace Damara;

/// <summary>
/// Converts binary floating-point numbers to decimal the way .NET did before .NET 11.
/// </summary>
/// <remarks>
/// From .NET 11 a cast such as <c>(decimal)0.01</c> keeps the exact binary value,
/// 0.0100000000000000002081668171, where earlier versions kept 15 significant digits for a
/// double (7 for a float) and so gave 0.01. These methods round to those digits, ties to even,
/// which is what the old cast did. Values outside the range of decimal throw, as the cast does.
/// See https://learn.microsoft.com/dotnet/core/compatibility/core-libraries/11/decimal-biginteger-floating-point-conversions.
/// </remarks>
public static class DecimalConversions
{
#nullable enable

    /// <summary>
    /// Converts a double to decimal, keeping 15 significant digits.
    /// </summary>
    /// <param name="value">The double.</param>
    /// <returns>
    /// The decimal: 0.01 for <c>0.01</c>, where <c>(decimal)0.01</c> gives 0.0100000000000000002081668171.
    /// </returns>
    /// <exception cref="OverflowException">The value is not a finite number a decimal can hold.</exception>
    public static decimal ToDecimalRound15(this double value) => Parse(value, "G15");

    /// <summary>
    /// Converts a float to decimal, keeping 7 significant digits.
    /// </summary>
    /// <param name="value">The float.</param>
    /// <returns>The decimal: 14.1 for <c>14.1f</c>.</returns>
    /// <exception cref="OverflowException">The value is not a finite number a decimal can hold.</exception>
    public static decimal ToDecimalRound7(this float value) => Parse(value, "G7");

    /// <summary>
    /// Formats a number to its significant digits and reads it back as a decimal.
    /// </summary>
    /// <typeparam name="T">The floating-point type.</typeparam>
    /// <param name="value">The number.</param>
    /// <param name="format">The G format that keeps the digits.</param>
    /// <returns>The decimal.</returns>
    /// <exception cref="OverflowException">The value is not a finite number a decimal can hold.</exception>
    private static decimal Parse<T>(T value, string format)
        where T : ISpanFormattable
    {
        Span<char> text = stackalloc char[32];
        value.TryFormat(text, out var length, format, CultureInfo.InvariantCulture);
        if (!decimal.TryParse(text[..length], NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
        {
            throw new OverflowException($"{text[..length]} is not a finite number a decimal can hold.");
        }

        return result;
    }
}
