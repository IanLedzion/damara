// <copyright file="BeforeCommitChangesArgs.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides properties for the before commit changes event, raised inside the store's
/// transaction after the unit of work has written its changes and before they are committed.
/// </summary>
public class BeforeCommitChangesArgs : EventArgs
{
}
