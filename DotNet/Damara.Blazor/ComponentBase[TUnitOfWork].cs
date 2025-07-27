// <copyright file="ComponentBase[TUnitOfWork].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Damara.Blazor;

/// <summary>
/// Base class for razor components. The class exposes a unit of work and service provider which exist for the duration of the component.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <seealso cref="OwningComponentBase" />
#pragma warning disable SA1649 // File name should match first type name
public abstract class ComponentBase<TUnitOfWork> : OwningComponentBase
#pragma warning restore SA1649 // File name should match first type name
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentBase{TUnitOfWork}"/> class.
    /// </summary>
    protected ComponentBase()
    {
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; private set; }

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