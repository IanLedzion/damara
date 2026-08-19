// <copyright file="MediaSizeQuery.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Blazor;

/// <summary>
/// Provides media queries for determining media sizes, using the Bootstrap scale.
/// </summary>
/// <remarks>
/// <para>
/// The constants come in two families. The unsuffixed ones are <b>minimum</b> widths: each is
/// true at its own breakpoint <b>and every size above it</b>, so they are cumulative and read as
/// "at least this wide". At 1400px, <see cref="Small"/>, <see cref="Medium"/> and
/// <see cref="Large"/> are all true. They do not identify which size the viewport is.
/// </para>
/// <para>
/// The <c>*Down</c> constants are <b>maximum</b> widths and are the complement of the
/// unsuffixed constant of the same name, so <see cref="SmallDown"/> matches exactly when
/// <see cref="Small"/> does not. Use these when the intent is "this size or narrower" -- for
/// example, <see cref="MediumDown"/> covers phones in both portrait and landscape.
/// </para>
/// </remarks>
public static class MediaSizeQuery
{
    /// <summary>
    /// Small devices (landscape phones, 576px and up).
    /// </summary>
    public const string Small = "(min-width: 576px)";

    /// <summary>
    /// Medium devices (tablets, 768px and up).
    /// </summary>
    public const string Medium = "(min-width: 768px)";

    /// <summary>
    /// Large devices (desktops, 992px and up).
    /// </summary>
    public const string Large = "(min-width: 992px)";

    /// <summary>
    /// Extra large devices (large desktops, 1200px and up).
    /// </summary>
    public const string ExtraLarge = "(min-width: 1200px)";

    /// <summary>
    /// Extra extra large devices (large desktops, 1600px and up).
    /// </summary>
    public const string ExtraExtraLarge = "(min-width: 1600px)";

    /// <summary>
    /// Narrower than small (portrait phones, below 576px).
    /// </summary>
    public const string SmallDown = "(max-width: 575.98px)";

    /// <summary>
    /// Narrower than medium (phones in portrait and landscape, below 768px).
    /// </summary>
    public const string MediumDown = "(max-width: 767.98px)";

    /// <summary>
    /// Narrower than large (phones and portrait tablets, below 992px).
    /// </summary>
    public const string LargeDown = "(max-width: 991.98px)";

    /// <summary>
    /// Narrower than extra large (below 1200px).
    /// </summary>
    public const string ExtraLargeDown = "(max-width: 1199.98px)";

    /// <summary>
    /// Narrower than extra extra large (below 1600px).
    /// </summary>
    public const string ExtraExtraLargeDown = "(max-width: 1599.98px)";
}
