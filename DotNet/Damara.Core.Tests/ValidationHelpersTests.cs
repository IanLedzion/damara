// <copyright file="ValidationHelpersTests.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Damara.Core.Tests;

/// <summary>
/// Provides tests for the <see cref="ValidationHelpers"/> class.
/// </summary>
[TestClass]
public class ValidationHelpersTests
{
    /// <summary>
    /// Tests that the BuildMessages behaves as expected.
    /// </summary>
    [TestMethod]
    public void BuildMessages_InvalidInstance_ValidResult()
    {
        var instance = new ObjectInstances.EntityInstance() { Name = "Name" };
        var validator = new ObjectInstances.EntityInstanceValidator(null);
        FluentValidation.Results.ValidationResult result;

        instance.Status = "Error";
        result = validator.Validate(instance);
        Assert.AreEqual(" - Error Status", result.BuildMessage(), "Message not correctly built");
        Assert.AreEqual(" - Error: Error Status", result.BuildMessage(true), "Message not correctly built");
    }

    /// <summary>
    /// Tests that the BuildJsonMessages behaves as expected.
    /// </summary>
    [TestMethod]
    public void BuildJsonMessages_InvalidInstance_ValidResult()
    {
        var instance = new ObjectInstances.EntityInstance() { Name = "Name" };
        var validator = new ObjectInstances.EntityInstanceValidator(null);
        FluentValidation.Results.ValidationResult result;

        instance.Status = "Error";
        result = validator.Validate(instance);
        var messages = result.BuildJsonMessageArray();

        var array = JArray.Parse(messages);
        var message = array[0];
        Assert.AreEqual("Error", message["Severity"], "Severity not set");
        Assert.AreEqual("Error Status", message["Message"], "Severity not set");
    }

    /// <summary>
    /// Tests that the GetMessagesFromJson behaves as expected.
    /// </summary>
    [TestMethod]
    public void GetMessagesFromJson_ValidJson_ValidResult()
    {
        var json = "[{\"Severity\":\"Error\",\"Message\":\"This is an error\"}]";
        var messages = json.GetMessagesFromJson();
        var message = messages.ElementAt(0);
        Assert.AreEqual("Error", message.Severity, "Severity not read");
        Assert.AreEqual("This is an error", message.Message, "Message not read");
    }

    /// <summary>
    /// Tests that the correct number of error failures is returned.
    /// </summary>
    [TestMethod]
    public void ErrorFailureCount_InvalidInstance_ValidResult()
    {
        var instance = new ObjectInstances.EntityInstance() { Name = "Name" };
        var validator = new ObjectInstances.EntityInstanceValidator(null);
        FluentValidation.Results.ValidationResult result;

        instance.Status = "Error";
        result = validator.Validate(instance);
        Assert.AreEqual(1, result.ErrorFailureCount(), "Invalid instance should have 1 error");
        Assert.AreEqual("Status", result.Errors[0].PropertyName, "Error property name not set");
        Assert.AreEqual("Error Status", result.Errors[0].ErrorMessage, "Error message not set");

        instance.Status = "None";
        result = validator.Validate(instance);
        Assert.AreEqual(0, result.ErrorFailureCount(), "Valid instance should have 1 error");
    }

    /// <summary>
    /// Tests that the correct number of Warning failures is returned.
    /// </summary>
    [TestMethod]
    public void WarningFailureCount_InvalidInstance_ValidResult()
    {
        var instance = new ObjectInstances.EntityInstance() { Name = "Name" };
        var validator = new ObjectInstances.EntityInstanceValidator(null);
        FluentValidation.Results.ValidationResult result;

        instance.Status = "Warning";
        result = validator.Validate(instance);
        Assert.AreEqual(1, result.WarningFailureCount(), "Invalid instance should have 1 Warning");
        Assert.AreEqual("Warning Status", result.Errors[0].ErrorMessage, "Error message not set");

        instance.Status = "None";
        result = validator.Validate(instance);
        Assert.AreEqual(0, result.WarningFailureCount(), "Valid instance should have 1 Warning");
    }

    /// <summary>
    /// Tests that the correct number of Info failures is returned.
    /// </summary>
    [TestMethod]
    public void InfoFailureCount_InvalidInstance_ValidResult()
    {
        var instance = new ObjectInstances.EntityInstance() { Name = "Name" };
        var validator = new ObjectInstances.EntityInstanceValidator(null);
        FluentValidation.Results.ValidationResult result;

        instance.Status = "Info";
        result = validator.Validate(instance);
        Assert.AreEqual(1, result.InfoFailureCount(), "Invalid instance should have 1 Info");
        Assert.AreEqual("Info Status", result.Errors[0].ErrorMessage, "Error message not set");

        instance.Status = "None";
        result = validator.Validate(instance);
        Assert.AreEqual(0, result.InfoFailureCount(), "Valid instance should have 1 Info");
    }
}
