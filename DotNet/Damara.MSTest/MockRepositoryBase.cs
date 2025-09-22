// <copyright file="MockRepositoryBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.MSTest;

public class MockRepositoryBase<TUnitOfWork, TEntity> : IRepository<TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    private readonly List<TEntity> entities;

    public MockRepositoryBase(TUnitOfWork unitOfWork, IEnumerable<TEntity> entities)
    {
        this.UnitOfWork = unitOfWork;
        this.entities = entities.ToList();
    }

    protected TUnitOfWork UnitOfWork { get; }

    public void Add(TEntity entity) => this.entities.Add(entity);

    public IEnumerable<TEntity> GetAll() => this.entities;

    public void Refresh(IEnumerable<TEntity> entities)
    {
    }

    public void Refresh(TEntity entity)
    {
    }

    public void Remove(TEntity entity) => this.entities.Remove(entity);
}
