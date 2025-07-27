// <copyright file="MockRepository.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Core.Tests.ObjectInstances;

/// <summary>
/// Provides a concrete repository implementation for LinqConnect data contexts.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
/// <typeparam name="TEntity">The entity type managed by the repository.</typeparam>
public abstract class MockRepository<TUnitOfWork, TEntity> : RepositoryBase<TUnitOfWork, TEntity>
    where TEntity : EntityBase
    where TUnitOfWork : MockUnitOfWork
{
    protected MockRepository(TUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }
}