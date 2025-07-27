// <copyright file="HangfireServiceAttribute.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

namespace Damara.Hangfire;

/// <summary>
/// When decorating an interface, indicates that it represents a Hangfire service contract.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
public sealed class HangfireServiceAttribute : Attribute
{
}