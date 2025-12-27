// <copyright file="FactoryBase.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides a base class for repositories.
/// </summary>
/// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
public abstract class FactoryBase<TUnitOfWork, TEntity>
    where TUnitOfWork : IUnitOfWork
    where TEntity : EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FactoryBase{TUnitOfWork, TEntity}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    protected FactoryBase(TUnitOfWork unitOfWork)
    {
        this.UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    protected TUnitOfWork UnitOfWork { get; private set; }

    /// <summary>
    /// Sets the private property.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    protected void SetPrivateProperty(TEntity entity, string propertyName, object value)
    {
        PropertyInfo propertyInfo = null;
        foreach (var type in typeof(TEntity).Ancestors(type => type.BaseType))
        {
            propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo != null)
            {
                break;
            }
        }

        propertyInfo.SetValue(entity, value);
    }
}