// <copyright file="EntityInstanceRepository.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// A test repository.
/// </summary>
/// <seealso cref="Damara.LinqConnectRepository{EntityInstance, TestDataContext, ConcreteUnitOfWork}" />
public class EntityInstanceRepository : MockRepository<MockUnitOfWork, EntityInstance>
{
    public EntityInstanceRepository(MockUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public override IQueryable<EntityInstance> GetAll() => throw new System.NotImplementedException();

    public override void Add(EntityInstance entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Refresh(IEnumerable<EntityInstance> entities)
    {
        throw new System.NotImplementedException();
    }

    public override void Refresh(EntityInstance entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Remove(EntityInstance entity)
    {
        throw new System.NotImplementedException();
    }
}
