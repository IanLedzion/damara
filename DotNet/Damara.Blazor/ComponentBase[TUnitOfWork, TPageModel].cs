// <copyright file="ComponentBase[TUnitOfWork, TPageModel].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

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
    /// Saves the changes.
    /// </summary>
    protected void SaveChanges()
    {
        this.UnitOfWork.SaveChanges();
    }

    /// <summary>
    /// Cancels the changes.
    /// </summary>
    protected void CancelChanges()
    {
        this.UnitOfWork.CancelChanges();
    }
}