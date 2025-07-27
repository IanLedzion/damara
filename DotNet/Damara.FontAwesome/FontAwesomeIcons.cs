// <copyright file="FontAwesomeIcons.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Damara.FontAwesome.Resources;

namespace Damara.FontAwesome;

/// <summary>
/// Provides constants.
/// </summary>
public static class FontAwesomeIcons
{
    private static readonly IconNames RegularIcons = new("fa-regular");
    private static readonly IconNames SolidIcons = new("fa-solid");

    /// <summary>
    /// Gets the regular iconss.
    /// </summary>
    public static IconNames Regular => RegularIcons;

    /// <summary>
    /// Gets the solid iconss.
    /// </summary>
    public static IconNames Solid => SolidIcons;

    /// <summary>
    /// Adds a CSS class.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <param name="cssClass">The CSS class.</param>
    /// <returns>
    /// The icon with CSS class.
    /// </returns>
    public static string AddCssClass(this string icon, string cssClass) => $"{icon} {cssClass}";

    /// <summary>
    /// Marks the icon as fixed width.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <returns>The fixed width icon.</returns>
    public static string FixedWidth(this string icon) => $"{icon} fa-fw";

    /// <summary>
    /// Scales the specified icon.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <param name="scale">The scale.</param>
    /// <returns>The scaled icon.</returns>
    public static string Scale(this string icon, int scale) => $"{icon} fa-{scale}x";

    /// <summary>
    /// Spins the specified icon.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <returns>The spinning icon.</returns>
    public static string Spin(this string icon) => $"{icon} fa-spin";
}