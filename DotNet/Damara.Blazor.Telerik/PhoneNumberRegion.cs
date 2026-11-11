// <copyright file="PhoneNumberRegion.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Blazor.Telerik;

/// <summary>
/// A region offered by <see cref="PhoneNumberEdit"/>: a country or territory with its calling code.
/// </summary>
public class PhoneNumberRegion
{
    /// <summary>
    /// Gets the ISO 3166-1 alpha-2 region code, such as <c>CH</c>.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Gets the region's name in the current UI culture.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the calling code with its plus sign, such as <c>+41</c>.
    /// </summary>
    public required string CallingCode { get; init; }

    /// <summary>
    /// Gets the text shown in the region list: the name and the calling code.
    /// </summary>
    public string Text => $"{this.Name} {this.CallingCode}";
}
