//-----------------------------------------------------------------------
// <copyright file="EntityValidatorTests.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Damara.Core.Tests.ObjectInstances;

namespace Damara.Core.Tests;

/// <summary>
/// Provides tests for the <see cref="EntityValidator{TEntity}"/> class.
/// </summary>
[TestClass]
public class EntityValidatorTests
{
    /// <summary>
    /// Tests for custom validation.
    /// </summary>
    [TestMethod]
    public void Validate_CustomValidation_ValidResult()
    {
        var instance = new EntityInstance();
        var validator = new EntityInstanceValidator(null);
        FluentValidation.Results.ValidationResult result;

        instance.Name = "Invalid";
        result = validator.Validate(instance);
        Assert.AreEqual(1, result.Errors.Count, "Invalid instance should have 1 error");
    }
}
