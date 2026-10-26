// <copyright file="ComponentBase[TUnitOfWork, TPageModel].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Damara.Blazor;

/// <summary>
/// Base class for razor components. The class exposes a unit of work and service provider which exist for the duration of the component.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TPageModel">The type of the page model.</typeparam>
/// <seealso cref="ComponentBase" />
#pragma warning disable SA1649 // File name should match first type name
public abstract class ComponentBase<TUnitOfWork, TPageModel> : OwningComponentBase
#pragma warning restore SA1649 // File name should match first type name
    where TUnitOfWork : IUnitOfWork
    where TPageModel : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentBase{TUnitOfWork, TPageModel}"/> class.
    /// </summary>
    protected ComponentBase()
    {
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; private set; }

    /// <summary>
    /// Gets the page model.
    /// </summary>
    protected TPageModel PageModel { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the page model was created.
    /// </summary>
    protected bool PageModelCreated { get; private set; }

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// </summary>
    protected sealed override void OnInitialized()
    {
        base.OnInitialized();
        this.UnitOfWork = this.ScopedServices.GetRequiredService<TUnitOfWork>();
    }

    /// <summary>
    /// Sets the page model.
    /// </summary>
    /// <param name="pageModel">The page model.</param>
    protected virtual void SetPageModel(TPageModel pageModel)
    {
        if (pageModel == null)
        {
            return;
        }

        this.PageModel = pageModel;
        this.PageModelCreated = true;
        this.OnPageModelSet();
    }

    /// <summary>
    /// Called when the page model has been set.
    /// </summary>
    protected virtual void OnPageModelSet()
    {
    }

    /// <summary>
    /// Replaces a child page model, such as the one behind a dialog, disposing the one it replaces.
    /// A subscribing page model handles every save and cancel of the component's unit of work until
    /// it is disposed, so one that is merely overwritten keeps acting on later, unrelated saves.
    /// </summary>
    /// <typeparam name="TChildPageModel">The type of the child page model.</typeparam>
    /// <param name="current">The child page model held now, if any.</param>
    /// <param name="replacement">The child page model to hold instead.</param>
    /// <returns>The replacement, to assign to the property that holds the child page model.</returns>
    protected TChildPageModel ReplaceChildPageModel<TChildPageModel>(TChildPageModel current, TChildPageModel replacement)
        where TChildPageModel : class, IDisposable
    {
        if (!ReferenceEquals(current, replacement))
        {
            current?.Dispose();
        }

        return replacement;
    }

    /// <summary>
    /// Releases a child page model the component has finished with, such as the one behind a dialog
    /// that has closed, disposing it so it stops handling the unit of work's save and cancel events.
    /// </summary>
    /// <typeparam name="TChildPageModel">The type of the child page model.</typeparam>
    /// <param name="current">The child page model held now, if any.</param>
    /// <returns><c>null</c>, to assign to the property that holds the child page model.</returns>
    protected TChildPageModel ReleaseChildPageModel<TChildPageModel>(TChildPageModel current)
        where TChildPageModel : class, IDisposable
    {
        return this.ReplaceChildPageModel(current, null);
    }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    protected void SaveChanges()
    {
        this.UnitOfWork.SaveChanges();
    }

    /// <summary>
    /// Cancels the changes.
    /// </summary>
    [Obsolete("EF Core change tracking does not support undo; kept for backwards compatibility.")]
    protected void CancelChanges()
    {
        this.UnitOfWork.CancelChanges();
    }
}