// <copyright file="ValidationHelpers.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using FluentValidation.Results;
using Newtonsoft.Json.Linq;

namespace Damara;

/// <summary>
/// Provides validation helper methods.
/// </summary>
public static class ValidationHelpers
{
    /// <summary>
    /// Determines whether a string is a valid email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns>
    ///   <c>true</c> if the string is a valid email; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidEmail(this string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Builds a validation message which can be displayed in a message .
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns>
    /// A message containing validation errors.
    /// </returns>
    public static string SerialiseJsonErrorCodes(this ValidationResult result)
    {
        var jErrors = new JArray();

        foreach (var failure in result.Errors)
        {
            var jFailure = new JObject
            {
                ["PropertyName"] = failure.PropertyName,
                ["ErrorCode"] = failure.ErrorCode,
                ["ErrorMessage"] = failure.ErrorMessage,
            };

            jFailure["AttemptedValue"] = failure.AttemptedValue is DateTime date ? (JToken)date.ToString("s") : (JToken)failure.AttemptedValue.ToString();

            jErrors.Add(jFailure);
        }

        var jObject = new JObject
        {
            ["Failures"] = jErrors,
        };

        return jObject.ToString();
    }

    /// <summary>
    /// Builds a validation message which can be displayed in a message .
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="includeSeverity">if set to <c>true</c> the failure severity is included in the message.</param>
    /// <returns>
    /// A message containing validation errors.
    /// </returns>
    public static string BuildMessage(this ValidationResult result, bool includeSeverity)
    {
        if (result.IsValid)
        {
            return null;
        }

        var builder = new StringBuilder();

        foreach (var error in result.Errors)
        {
            if (builder.Length != 0)
            {
                builder.AppendLine();
            }

            if (includeSeverity)
            {
                builder.AppendFormat(" - {0}: {1}", error.Severity, error.ErrorMessage);
            }
            else
            {
                builder.AppendFormat(" - {0}", error.ErrorMessage);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Builds a Json validation message array.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns>
    /// A Json array of errors.
    /// </returns>
    public static string BuildJsonMessageArray(this ValidationResult result)
    {
        var jErrors = new JArray();

        if (result.IsValid)
        {
            return jErrors.ToString();
        }

        foreach (var failure in result.Errors)
        {
            var jFailure = new JObject
            {
                ["Severity"] = failure.Severity.ToString(),
                ["Message"] = failure.ErrorMessage,
            };
            jErrors.Add(jFailure);
        }

        return jErrors.ToString(formatting: Newtonsoft.Json.Formatting.None);
    }

    /// <summary>
    /// Gets the messages from json.
    /// </summary>
    /// <param name="json">The json.</param>
    /// <returns>A collection of json error messages.</returns>
    public static IEnumerable<Validation.ErrorMessage> GetMessagesFromJson(this string json)
    {
        return JsonConvert.DeserializeObject<List<Validation.ErrorMessage>>(json);
    }

    /// <summary>
    /// Builds a validation message which can be displayed in a message .
    /// </summary>
    /// <param name="result">The result.</param>
    /// <returns>A message containing validation errors.</returns>
    public static string BuildMessage(this ValidationResult result)
    {
        return result.BuildMessage(false);
    }

    /// <summary>
    /// Returns the number of failures in the Error severity status.
    /// </summary>
    /// <param name="result">The validation result.</param>
    /// <returns>Number of failures in the Error severity status.</returns>
    public static int ErrorFailureCount(this ValidationResult result)
    {
        return result.Errors.Count(e => e.Severity == FluentValidation.Severity.Error);
    }

    /// <summary>
    /// Returns the number of failures in the Warning severity status.
    /// </summary>
    /// <param name="result">The validation result.</param>
    /// <returns>Number of failures in the Warning severity status.</returns>
    public static int WarningFailureCount(this ValidationResult result)
    {
        return result.Errors.Count(e => e.Severity == FluentValidation.Severity.Warning);
    }

    /// <summary>
    /// Returns the number of failures in the Info severity status.
    /// </summary>
    /// <param name="result">The validation result.</param>
    /// <returns>Number of failures in the Info severity status.</returns>
    public static int InfoFailureCount(this ValidationResult result)
    {
        return result.Errors.Count(e => e.Severity == FluentValidation.Severity.Info);
    }

    /// <summary>
    /// Adds an error failure to the context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="message">The message.</param>
    public static void AddError<T>(this ValidationContext<T> context, string message)
    {
        var failure = new ValidationFailure(context.PropertyPath, message)
        {
            Severity = Severity.Error,
        };

        context.AddFailure(failure);
    }

    /// <summary>
    /// Adds a warning failure to the context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="message">The message.</param>
    public static void AddWarning<T>(this ValidationContext<T> context, string message)
    {
        var failure = new ValidationFailure(context.PropertyPath, message)
        {
            Severity = Severity.Warning,
        };

        context.AddFailure(failure);
    }

    /// <summary>
    /// Adds an information failure to the context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="message">The message.</param>
    public static void AddInfo<T>(this ValidationContext<T> context, string message)
    {
        var failure = new ValidationFailure(context.PropertyPath, message)
        {
            Severity = Severity.Info,
        };

        context.AddFailure(failure);
    }
}