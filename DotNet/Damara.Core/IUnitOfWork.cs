// <copyright file="IUnitOfWork.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

public interface IUnitOfWork
{
    event EventHandler<AfterCancelChangesArgs> AfterCancelChanges;

    event EventHandler<AfterSaveChangesArgs> AfterSaveChanges;

    event EventHandler<BeforeCancelChangesArgs> BeforeCancelChanges;

    event EventHandler<BeforeSaveChangesArgs> BeforeSaveChanges;

    int CreateSequence { get; }

    bool HasChanges { get; }

    void CancelChanges();

    IEnumerable<EntityBase> GetAddedEntities();

    IEnumerable<EntityBase> GetDeletedEntities();

    IEnumerable<EntityBase> GetModifiedEntities();

    void Refresh(IEnumerable<EntityBase> entities);

    void SaveChanges();
}