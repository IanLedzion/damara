// <copyright file="MockUnitOfWork.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Concrete unit of work which should be loaded into a descriptor.
/// </summary>
public class MockUnitOfWork : UnitOfWorkBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MockUnitOfWork"/> class.
    /// </summary>
    public MockUnitOfWork()
    {
    }

    protected override bool HasChangesCore => throw new System.NotImplementedException();

    protected override void CancelChangesCore()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<EntityBase> GetAddedEntitiesCore()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<EntityBase> GetDeletedEntitiesCore()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<EntityBase> GetModifiedEntitiesCore()
    {
        throw new System.NotImplementedException();
    }

    protected override void RefreshCore(IEnumerable<EntityBase> entities)
    {
        throw new System.NotImplementedException();
    }

    protected override void SaveChangesCore()
    {
        throw new System.NotImplementedException();
    }
}
