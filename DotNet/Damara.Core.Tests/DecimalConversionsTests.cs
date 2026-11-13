// <copyright file="DecimalConversionsTests.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using System.Globalization;

namespace Damara.Core.Tests;

/// <summary>
/// Provides tests for the <see cref="DecimalConversions"/> class.
/// </summary>
[TestClass]
public class DecimalConversionsTests
{
    /// <summary>
    /// Tests that a double keeps 15 significant digits, as the cast did before .NET 11.
    /// </summary>
    /// <param name="value">The double.</param>
    /// <param name="expected">The decimal, as text.</param>
    [TestMethod]
    [DataRow(0.01, "0.01")]
    [DataRow(1.23, "1.23")]
    [DataRow(-123.4567, "-123.4567")]
    [DataRow(0.0001, "0.0001")]
    [DataRow(1e-8, "0.00000001")]
    [DataRow(0.1 + 0.2, "0.3")]
    [DataRow(1234567.891, "1234567.891")]
    [DataRow(0.0, "0")]
    public void ToDecimalRound15_KeepsFifteenDigits(double value, string expected)
    {
        Assert.AreEqual(decimal.Parse(expected, CultureInfo.InvariantCulture), value.ToDecimalRound15());
    }

    /// <summary>
    /// Tests that the cast itself no longer rounds, which is why the method exists.
    /// </summary>
    [TestMethod]
    public void ToDecimalRound15_DiffersFromTheNet11Cast()
    {
        var value = 0.01;
        Assert.AreNotEqual(0.01m, (decimal)value, "From .NET 11 the cast keeps the exact binary value.");
        Assert.AreEqual(0.01m, value.ToDecimalRound15());
    }

    /// <summary>
    /// Tests that a float keeps 7 significant digits.
    /// </summary>
    [TestMethod]
    public void ToDecimalRound7_KeepsSevenDigits()
    {
        Assert.AreEqual(14.1m, 14.1f.ToDecimalRound7());
        Assert.AreEqual(0.3m, 0.3f.ToDecimalRound7());
    }

    /// <summary>
    /// Tests that what a decimal cannot hold throws, as the cast does.
    /// </summary>
    /// <param name="value">The double.</param>
    [TestMethod]
    [DataRow(double.NaN)]
    [DataRow(double.PositiveInfinity)]
    [DataRow(1e30)]
    public void ToDecimalRound15_OutOfRange_Throws(double value)
    {
        Assert.ThrowsExactly<OverflowException>(() => value.ToDecimalRound15());
    }
}
