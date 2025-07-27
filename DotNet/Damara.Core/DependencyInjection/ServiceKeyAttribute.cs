// <copyright file="ServiceKeyAttribute.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.DependencyInjection;

/// <summary>
/// Classes decorated with this attribute are servcies with a service key.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ServiceKeyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceKeyAttribute"/> class.
    /// </summary>
    /// <param name="key">The key.</param>
    public ServiceKeyAttribute(string key)
    {
        this.Key = key;
    }

    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <value>
    /// The key.
    /// </value>
    public string Key { get; }
}