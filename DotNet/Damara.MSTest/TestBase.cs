// <copyright file="TestBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Damara.MSTest;

/// <summary>
/// Base class for tests.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work base.</typeparam>
public abstract class TestBase<TUnitOfWork>
    where TUnitOfWork : class, IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase{TUnitOfWork}"/> class.
    /// </summary>
    protected TestBase()
    {
        var unitOfWork = Substitute.For<TUnitOfWork>();
        this.ConfigureUnitOfWork(unitOfWork);
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Tests initialize.
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        this.TestInitializeCore();
    }

    /// <summary>
    /// Tests cleanup.
    /// </summary>
    [TestCleanup]
    public void TestCleanup()
    {
        this.TestCleanupCore();
        (this.UnitOfWork as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Configures the unit of work.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected virtual void ConfigureUnitOfWork(TUnitOfWork unitOfWork)
    {
    }

    /// <summary>
    /// Tests initialize core method.
    /// </summary>
    protected virtual void TestInitializeCore()
    {
    }

    /// <summary>
    /// Tests cleanup core method.
    /// </summary>
    protected virtual void TestCleanupCore()
    {
    }
}