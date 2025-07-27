// <copyright file="RazorHelpers.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Damara.Blazor;

/// <summary>
/// Provides helper mehtods for Razor components.
/// </summary>
public static class RazorHelpers
{
    /// <summary>
    /// Highlights the search text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="highlightText">The text to highlight.</param>
    /// <param name="hightlightCssClass">The hightlight CSS class.</param>
    /// <returns>
    /// The text with search text highlighted.
    /// </returns>
    public static MarkupString HighlightSearch(string text, string highlightText, bool ignoreWhitespace, string hightlightCssClass)
    {
        var cleanText = RemoveDiacritics(text);
        var cleanHighlightText = RemoveDiacritics(highlightText);

        var pos = IndexOfNoSpace(cleanText, cleanHighlightText, ignoreWhitespace);

        if (!pos.HasValue)
        {
            return new MarkupString(text);
        }

        var startPosition = pos.Value.StartPosition;
        var endPosition = pos.Value.EndPosition;

        var builder = new StringBuilder();
        for (var i = 0; i < text.Length; i++)
        {
            if (i == startPosition)
            {
                builder.Append($"<span class=\"{hightlightCssClass}\">");
            }

            builder.Append(text[i]);

            if (i == endPosition)
            {
                builder.Append("</span>");
            }
        }

        return new MarkupString(builder.ToString());
    }

    /// <summary>
    /// Removes the diacritics.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>Text without diacritics.</returns>
    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Gets the start and end of a comparison string, optionally ignoring whitespace.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="compareTo">The compare to.</param>
    /// <param name="ignoreWhitespace">if set to <c>true</c> [ignore whitespace].</param>
    /// <returns>The start and end of the comparison text, or null if no match was found.</returns>
    private static (int StartPosition, int EndPosition)? IndexOfNoSpace(string text, string compareTo, bool ignoreWhitespace)
    {
        var comparing = false;
        var startPos = 0;
        var compareToPos = 0;

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            if (char.IsWhiteSpace(c) && ignoreWhitespace)
            {
                continue;
            }

            if (char.ToUpperInvariant(text[i]) == char.ToUpperInvariant(compareTo[compareToPos]))
            {
                if (!comparing)
                {
                    startPos = i;
                    comparing = true;
                }

                if (compareToPos == compareTo.Length - 1)
                {
                    return (startPos, i);
                }

                compareToPos++;
            }
            else
            {
                comparing = false;
                compareToPos = 0;
            }
        }

        return null;
    }
}