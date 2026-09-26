// <copyright file="SaveChangesFailedArgs.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara;

/// <summary>
/// Provides properties for the save changes failed event, raised when a save was rolled back.
/// </summary>
public class SaveChangesFailedArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveChangesFailedArgs"/> class.
    /// </summary>
    /// <param name="exception">The exception that failed the save.</param>
    public SaveChangesFailedArgs(Exception exception)
    {
        this.Exception = exception;
    }

    /// <summary>
    /// Gets the exception that failed the save.
    /// </summary>
    public Exception Exception { get; }
}
