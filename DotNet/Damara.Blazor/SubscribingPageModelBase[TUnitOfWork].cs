// <copyright file="SubscribingPageModelBase[TUnitOfWork].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using Serilog;

namespace Damara.Blazor;

/// <summary>
/// Provides a base page model class which scubscribes to data layer events.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Generic type")]
public abstract class SubscribingPageModelBase<TUnitOfWork> : SubscribingServiceBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribingPageModelBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    protected SubscribingPageModelBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribingPageModelBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="isNew">if set to <c>true</c> [is new].</param>
    protected SubscribingPageModelBase(TUnitOfWork unitOfWork, ILogger logger, bool isNew)
        : this(unitOfWork, logger)
    {
        this.IsNew = isNew;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is new.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
    /// </value>
    public bool IsNew { get; private set; }

    /// <summary>
    /// Occurs after changes are saved.
    /// </summary>
    protected override void OnAfterSaveChanges()
    {
        this.IsNew = false;

        base.OnAfterSaveChanges();
    }
}