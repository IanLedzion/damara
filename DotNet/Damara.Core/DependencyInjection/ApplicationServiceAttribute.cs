// <copyright file="ApplicationServiceAttribute.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.DependencyInjection;

/// <summary>
/// Interfaces decorated with this attribute are application services.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class ApplicationServiceAttribute : Attribute
{
}