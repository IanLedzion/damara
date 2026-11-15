// <copyright file="PageModelBase[TUnitOfWork].cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using ILogger = Serilog.ILogger;

namespace Damara.Blazor;

/// <summary>
/// Provides a base page model class.
/// </summary>
/// <typeparam name="TUnitOfWork">Type of the unit of work.</typeparam>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Generic type")]
public abstract class PageModelBase<TUnitOfWork> : ServiceBase<TUnitOfWork>
    where TUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageModelBase{TUnitOfWork}" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    protected PageModelBase(TUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger)
    {
    }
}