// <copyright file="ValidationLanguageManager.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Collections;
using System.Globalization;
using System.Resources;
using FluentValidation.Resources;

namespace Damara;

/// <summary>
/// Provides methods for managing languages in FluentValidation.
/// </summary>
public sealed class ValidationLanguageManager : LanguageManager
{
    /// <summary>
    /// The manager instance, lazy loaded.
    /// </summary>
    private static readonly Lazy<ValidationLanguageManager> LazyInstance = new(() => new ValidationLanguageManager());

    /// <summary>
    /// Prevents a default instance of the <see cref="ValidationLanguageManager"/> class from being created.
    /// </summary>
    private ValidationLanguageManager()
    {
        ValidatorOptions.Global.LanguageManager = this;
    }

    /// <summary>
    /// Gets the instance.
    /// </summary>
    public static ValidationLanguageManager Instance => LazyInstance.Value;

    /// <summary>
    /// Registers resources as codes.
    /// </summary>
    /// <param name="resourceManager">The resource manager.</param>
    /// <param name="cultures">The cultures.</param>
    public void RegisterResources(ResourceManager resourceManager, IEnumerable<CultureInfo> cultures)
    {
        if (resourceManager == null)
        {
            throw new ArgumentNullException(nameof(resourceManager), "Resource Manager cannot be null");
        }

        if (cultures == null)
        {
            throw new ArgumentNullException(nameof(cultures), "Cultures cannot be null");
        }

        foreach (var culture in cultures)
        {
            var resourceSet = resourceManager.GetResourceSet(culture, true, true);

            foreach (DictionaryEntry r in resourceSet)
            {
                this.AddTranslation(culture.Name, (string)r.Key, (string)r.Value);
            }
        }
    }
}