// <copyright file="PageErrorMessage.razor.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;

namespace Damara.Blazor.Telerik;

/// <summary>
/// Displays error messages on a page.
/// </summary>
/// <seealso cref="ComponentBase" />
public partial class PageErrorMessage
{
    /// <summary>
    /// Gets or sets the header.
    /// </summary>
    [Parameter]
    public RenderFragment Header { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    [Parameter]
    public RenderFragment Message { get; set; }
}