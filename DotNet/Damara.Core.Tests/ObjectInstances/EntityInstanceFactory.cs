// <copyright file="EntityInstanceFactory.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Entity instance factory.
/// </summary>
public class EntityInstanceFactory : FactoryBase<MockUnitOfWork, EntityInstance>
{
    public EntityInstanceFactory(MockUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>An entity instance.</returns>
    public EntityInstance Create()
    {
        return new EntityInstance();
    }
}
