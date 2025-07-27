// <copyright file="TestBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Damara.MSTest;

/// <summary>
/// Base class for tests.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the nit of work base.</typeparam>
public abstract class TestBase<TUnitOfWork>
    where TUnitOfWork : UnitOfWorkBase
{
    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; private set; }

    /// <summary>
    /// Tests initialize.
    /// </summary>
    [TestInitialize]
    public virtual void TestInitialize()
    {
        this.CreateUnitOfWork();
        this.TestInitializeCore();
    }

    /// <summary>
    /// Tests cleanup.
    /// </summary>
    [TestCleanup]
    public virtual void TestCleanup()
    {
        this.TestCleanupCore();
        this.UnitOfWork.Dispose();
        this.UnitOfWork = null;
    }

    /// <summary>
    /// Creates the unit of work.
    /// </summary>
    protected abstract void CreateUnitOfWork();

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