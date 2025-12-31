// <copyright file="ValidationLanguageManagerTests.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using Damara.Core.Tests.ObjectInstances;

namespace Damara.Core.Tests;

/// <summary>
/// Provides tests for the <see cref="ValidationLanguageManager"/> class.
/// </summary>
[TestClass]
public class ValidationLanguageManagerTests
{
    private static readonly IEnumerable<CultureInfo> Cultures = new List<CultureInfo>() { new("en"), new("fr") };

    /// <summary>
    /// Tests that an exception is thrown when sending a null resource manager.
    /// </summary>
    [TestMethod]
    public void RegisterResources_NullResourceManager_Exception()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ValidationLanguageManager.Instance.RegisterResources(null, Cultures));
    }

    /// <summary>
    /// Tests that an exception is thrown when sending a null culture collection.
    /// </summary>
    [TestMethod]
    public void RegisterResources_NullCultures_Exception()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ValidationLanguageManager.Instance.RegisterResources(ValidationLanauageResources.ResourceManager, null));
    }

    /// <summary>
    /// Tests that when correctly registering resources the validation messages are used.
    /// </summary>
    [TestMethod]
    public void RegisterResources_ValidArguments_Valid()
    {
        ValidationLanguageManager.Instance.RegisterResources(ValidationLanauageResources.ResourceManager, Cultures);

        var target = new ValidationTarget();
        var validator = new ValidationTargetValidator();
        ValidationResult result;

        var en = new CultureInfo("en");
        var fr = new CultureInfo("fr");
        var frCH = new CultureInfo("fr-CH");

        // Validate in English
        CultureInfo.CurrentUICulture = en;
        result = validator.Validate(target);
        Assert.AreEqual("Error message", result.Errors[0].ErrorMessage, "Error message not set in English");

        // Validate in French
        CultureInfo.CurrentUICulture = fr;
        result = validator.Validate(target);
        Assert.AreEqual("Message erreur", result.Errors[0].ErrorMessage, "Error message not set in French");

        // Validate in French (Switzerland)
        CultureInfo.CurrentUICulture = frCH;
        result = validator.Validate(target);
        Assert.AreEqual("Message erreur", result.Errors[0].ErrorMessage, "Error message not set in French");

        // Validate when overwriting a default message in English
        CultureInfo.CurrentUICulture = en;
        target.Name = null;
        result = validator.Validate(target);
        Assert.AreEqual("You cannot have 'Name' equal to null", result.Errors[0].ErrorMessage, "Error message not set in French");
    }
}
