// <copyright file="MediaSizeQuery.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Blazor;

/// <summary>
/// Provides media queries for determining media sizes, using the Bootstrap scale.
/// </summary>
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
}