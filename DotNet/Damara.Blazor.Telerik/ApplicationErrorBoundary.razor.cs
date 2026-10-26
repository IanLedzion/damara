// <copyright file="ApplicationErrorBoundary.razor.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Serilog;

namespace Damara.Blazor.Telerik;

/// <summary>
/// Error boundary which logs unhandled exceptions with a support reference and informs
/// the user via a dialog, replacing the default "yellow bar" experience. The page stays on
/// screen behind the dialog rather than being replaced by an error message, and the
/// boundary recovers automatically when the user navigates to another page.
/// </summary>
/// <remarks>
/// The renderer disposes a boundary's content when it hands the boundary an exception, so
/// the page is re-created rather than preserved; what the user sees is the page as freshly
/// loaded, with the dialog over it. The re-creation happens by recovering the boundary as
/// soon as the exception has been noted, while the dialog is driven by the boundary's own
/// copy of it. If the page fails again while it is being re-created, or keeps failing in
/// quick succession (a page whose load itself fails), the boundary stops re-creating it
/// and shows an error message in its place instead, so a page that cannot render does not
/// loop or bring the circuit down.
/// </remarks>
/// <seealso cref="ErrorBoundaryBase" />
public partial class ApplicationErrorBoundary : ErrorBoundaryBase, IDisposable
{
    /// <summary>
    /// How many exceptions within <see cref="RepeatWindow"/> mean the page cannot be shown.
    /// </summary>
    private const int RepeatLimit = 3;

    /// <summary>
    /// The window over which repeated exceptions are counted.
    /// </summary>
    private static readonly TimeSpan RepeatWindow = TimeSpan.FromSeconds(10);

    /// <summary>
    /// When the recent exceptions occurred.
    /// </summary>
    private readonly Queue<DateTime> recentErrors = new();

    /// <summary>
    /// Gets or sets the dialog title.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Unexpected error";

    /// <summary>
    /// Gets or sets the message describing what happened.
    /// </summary>
    [Parameter]
    public string Message { get; set; } = "Sorry - an unexpected error has occurred. The action you were performing may not have completed.";

    /// <summary>
    /// Gets or sets the message asking the user to inform support.
    /// </summary>
    [Parameter]
    public string SupportMessage { get; set; } = "Please inform support, quoting the error reference below.";

    /// <summary>
    /// Gets or sets the text of the continue button.
    /// </summary>
    [Parameter]
    public string ContinueButtonText { get; set; } = "Continue";

    /// <summary>
    /// Gets or sets the text of the reload button.
    /// </summary>
    [Parameter]
    public string ReloadButtonText { get; set; } = "Reload page";

    /// <summary>
    /// Gets or sets a value indicating whether the dialog shows the exception and its
    /// diagnostics (type, message and stack trace). Intended for internal applications
    /// whose users can act on the details; leave off for public-facing audiences.
    /// </summary>
    [Parameter]
    public bool ShowDiagnostics { get; set; }

    /// <summary>
    /// Gets the exception the dialog is showing, or null when the dialog is closed. This is
    /// the boundary's own copy: the base class's exception is cleared as soon as the page is
    /// re-created, while the dialog stays until the user dismisses it.
    /// </summary>
    protected Exception ShownException { get; private set; }

    /// <summary>
    /// Gets the reference the user can quote to support, correlating the dialog with the log entry.
    /// </summary>
    protected string ErrorReference { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the page has been given up on and an error message is
    /// shown in its place: it failed again while being re-created, or failed repeatedly.
    /// </summary>
    protected bool ChildContentFailed { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the render that re-creates the page is still
    /// in progress. An exception arriving while it is comes from that re-creation.
    /// </summary>
    private bool RecreatingChildContent { get; set; }

    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    [Inject]
    private ILogger Logger { get; set; }

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        this.NavigationManager.LocationChanged -= this.HandleLocationChanged;
    }

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        this.NavigationManager.LocationChanged += this.HandleLocationChanged;
    }

    /// <summary>
    /// Method invoked after each time the component has been rendered. A render that reaches
    /// this point completed, so an exception arriving after it did not come from re-creating
    /// the page.
    /// </summary>
    /// <param name="firstRender">Whether this is the first render.</param>
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        this.RecreatingChildContent = false;
    }

    /// <summary>
    /// Logs the exception with a support reference, shows the dialog, and re-creates the page
    /// behind it unless the page has shown it cannot be rendered.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override Task OnErrorAsync(Exception exception)
    {
        this.ErrorReference = string.Format(
            "{0:yyyyMMdd-HHmmss}-{1}",
            DateTime.UtcNow,
            Guid.NewGuid().ToString("N")[..6].ToUpperInvariant());

        this.Logger
            .ForContext(this.GetType())
            .ForContext("ErrorReference", this.ErrorReference)
            .Error(exception, "Unhandled exception {ErrorReference} at {Location}", this.ErrorReference, this.NavigationManager.Uri);

        this.ShownException = exception;

        var now = DateTime.UtcNow;
        this.recentErrors.Enqueue(now);
        while (this.recentErrors.Count > 0 && now - this.recentErrors.Peek() > RepeatWindow)
        {
            this.recentErrors.Dequeue();
        }

        if (this.RecreatingChildContent || this.recentErrors.Count >= RepeatLimit)
        {
            // The page threw while being re-created, or keeps throwing: it cannot be shown.
            // Leave the base class's exception in place so the error message renders instead.
            this.ChildContentFailed = true;

            this.Logger
                .ForContext(this.GetType())
                .ForContext("ErrorReference", this.ErrorReference)
                .Warning("Page at {Location} cannot be rendered after {ErrorReference}; showing the error message in its place", this.NavigationManager.Uri, this.ErrorReference);
        }
        else
        {
            _ = this.RecreateChildContentAsync();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Recovers the boundary so the page is re-created behind the dialog. The base class
    /// records the exception only after <see cref="OnErrorAsync"/> has returned, so this
    /// yields first; the continuation resumes on the renderer's synchronization context
    /// once that has happened. (InvokeAsync would run inline, before it.)
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task RecreateChildContentAsync()
    {
        await Task.Yield();
        this.RecreatingChildContent = true;
        this.Recover();
    }

    /// <summary>
    /// Closes the dialog. If the page had been given up on, it is tried once more.
    /// </summary>
    private void Continue()
    {
        this.ShownException = null;
        this.ChildContentFailed = false;
        this.recentErrors.Clear();

        if (this.CurrentException != null)
        {
            this.Recover();
        }
        else
        {
            this.StateHasChanged();
        }
    }

    /// <summary>
    /// Performs a full reload of the current page.
    /// </summary>
    private void Reload()
    {
        this.NavigationManager.NavigateTo(this.NavigationManager.Uri, forceLoad: true);
    }

    /// <summary>
    /// Closes the dialog and recovers the boundary when the user navigates away from the
    /// page that errored.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The event arguments.</param>
    private void HandleLocationChanged(object sender, LocationChangedEventArgs args)
    {
        if (this.ShownException != null || this.CurrentException != null)
        {
            _ = this.InvokeAsync(this.Continue);
        }
    }
}
