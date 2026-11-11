// <copyright file="PhoneNumberEdit.razor.cs" company="Ian Ledzion.">
// Copyright © Ian Ledzion. All rights reserved.
// </copyright>

using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PhoneNumbers;

namespace Damara.Blazor.Telerik;

/// <summary>
/// Edits a phone number stored in E.164 format (<c>+41791234567</c>): the user picks the region from a list and types
/// the number as it is written there, and the number is shown in that region's own format (<c>079 123 45 67</c>).
/// </summary>
/// <remarks>
/// A stored value that is not E.164 yet (free text from before) is read with <see cref="DefaultRegion"/> and written
/// back as E.164 once edited. Text that cannot be read as a number is passed on unchanged, so the form's validation
/// (FluentValidation's <c>PhoneNumber</c> rule) reports it.
/// </remarks>
public partial class PhoneNumberEdit
{
    private static readonly PhoneNumberUtil PhoneNumberUtil = PhoneNumberUtil.GetInstance();

    private string lastValue;
    private PhoneNumberRegion[] regions;

    /// <summary>
    /// Gets or sets the phone number in E.164 format; empty when there is none.
    /// </summary>
    [Parameter]
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the callback raised when the phone number changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets the expression identifying the bound value, so the form's validation sees changes to it.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> ValueExpression { get; set; }

    /// <summary>
    /// Gets or sets the region selected when there is no number and used to read numbers without a calling code.
    /// </summary>
    [Parameter]
    public string DefaultRegion { get; set; } = "CH";

    /// <summary>
    /// Gets or sets the regions listed first, in this order, before the others sorted by name.
    /// </summary>
    [Parameter]
    public IEnumerable<string> PreferredRegions { get; set; } = ["CH", "LI", "DE", "FR", "IT", "AT"];

    /// <summary>
    /// Gets or sets the function naming a region in the user's language from its ISO 3166-1 alpha-2 code; when it is
    /// not set or returns nothing, the region's English name is used.
    /// </summary>
    [Parameter]
    public Func<string, string> RegionName { get; set; }

    /// <summary>
    /// Gets or sets the id of the number's text box, for a <c>label</c>'s <c>for</c>.
    /// </summary>
    [Parameter]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the phone number can be edited.
    /// </summary>
    [Parameter]
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the width of the whole component.
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the width of the region list.
    /// </summary>
    [Parameter]
    public string RegionWidth { get; set; } = "90px";

    /// <summary>
    /// Gets or sets the edit context of the surrounding form, told when the number changes.
    /// </summary>
    [CascadingParameter]
    private EditContext EditContext { get; set; }

    /// <summary>
    /// Gets the regions offered: the preferred ones first, then the others by name.
    /// </summary>
    private PhoneNumberRegion[] Regions => this.regions ??= GetRegions(this.PreferredRegions, this.RegionName);

    /// <summary>
    /// Gets or sets the selected region code.
    /// </summary>
    private string RegionCode { get; set; }

    /// <summary>
    /// Gets or sets the number as shown and typed.
    /// </summary>
    private string Text { get; set; }

    /// <summary>
    /// Gets an example number of the selected region in its own format, as a hint.
    /// </summary>
    private string Placeholder
    {
        get
        {
            var example = PhoneNumberUtil.GetExampleNumberForType(this.RegionCode, PhoneNumberType.MOBILE);
            return example == null ? string.Empty : PhoneNumberUtil.Format(example, PhoneNumberFormat.NATIONAL);
        }
    }

    /// <summary>
    /// Reads a value that changed outside the component into the region and the text.
    /// </summary>
    protected override void OnParametersSet()
    {
        if (this.RegionCode != null && this.Value == this.lastValue)
        {
            return;
        }

        this.lastValue = this.Value;
        this.RegionCode = this.DefaultRegion;
        this.Text = this.Value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.Value))
        {
            this.Text = string.Empty;
            return;
        }

        if (TryParse(this.Value, this.DefaultRegion, out var number))
        {
            this.RegionCode = RegionOf(number, this.DefaultRegion);
            this.Text = PhoneNumberUtil.IsValidNumber(number)
                ? PhoneNumberUtil.Format(number, PhoneNumberFormat.NATIONAL)
                : this.Value;
        }
    }

    /// <summary>
    /// Parses a phone number, reading it as a number of <paramref name="region"/> when it has no calling code.
    /// </summary>
    /// <param name="text">The text to parse.</param>
    /// <param name="region">The region code assumed for a number without a calling code.</param>
    /// <param name="number">The parsed number.</param>
    /// <returns><c>true</c> if the text could be read as a phone number; otherwise, <c>false</c>.</returns>
    private static bool TryParse(string text, string region, out PhoneNumber number)
    {
        try
        {
            number = PhoneNumberUtil.Parse(text, region);
            return true;
        }
        catch (NumberParseException)
        {
            number = null;
            return false;
        }
    }

    /// <summary>
    /// Gets the region a number belongs to, falling back to the main region of its calling code, then to
    /// <paramref name="fallback"/>.
    /// </summary>
    /// <param name="number">The phone number.</param>
    /// <param name="fallback">The region code used when the number's region cannot be told.</param>
    /// <returns>The region code.</returns>
    private static string RegionOf(PhoneNumber number, string fallback)
    {
        var region = PhoneNumberUtil.GetRegionCodeForNumber(number);
        if (string.IsNullOrEmpty(region) || region == "ZZ")
        {
            region = PhoneNumberUtil.GetRegionCodeForCountryCode(number.CountryCode);
        }

        return string.IsNullOrEmpty(region) || region == "ZZ" ? fallback : region;
    }

    /// <summary>
    /// Gets the regions libphonenumber supports: the preferred regions first, then the others by name in the current
    /// UI culture.
    /// </summary>
    /// <param name="preferredRegions">The region codes listed first.</param>
    /// <param name="regionName">The function naming a region in the user's language, or <c>null</c>.</param>
    /// <returns>The regions.</returns>
    private static PhoneNumberRegion[] GetRegions(IEnumerable<string> preferredRegions, Func<string, string> regionName)
    {
        var preferred = (preferredRegions ?? []).ToArray();

        return PhoneNumberUtil
            .GetSupportedRegions()
            .Select(code => new PhoneNumberRegion
            {
                Code = code,
                Name = regionName?.Invoke(code) is { Length: > 0 } name ? name : EnglishName(code),
                CallingCode = $"+{PhoneNumberUtil.GetCountryCodeForRegion(code)}",
            })
            .OrderBy(region => Array.IndexOf(preferred, region.Code) is var index and >= 0 ? index : int.MaxValue)
            .ThenBy(region => region.Name, StringComparer.Create(CultureInfo.CurrentUICulture, ignoreCase: true))
            .ToArray();
    }

    /// <summary>
    /// Gets a region's English name, or the region code when .NET does not know the region. (.NET names regions in
    /// the user's language only when it matches the operating system's, so English is the one name it gives reliably.)
    /// </summary>
    /// <param name="code">The region code.</param>
    /// <returns>The region name.</returns>
    private static string EnglishName(string code)
    {
        try
        {
            return new RegionInfo(code).EnglishName;
        }
        catch (ArgumentException)
        {
            return code;
        }
    }

    /// <summary>
    /// Reads the number for the region from the list and builds the value from scratch.
    /// </summary>
    /// <param name="regionCode">The region code chosen.</param>
    private async Task OnRegionChanged(string regionCode)
    {
        this.RegionCode = regionCode ?? this.DefaultRegion;
        await this.CommitAsync();
    }

    /// <summary>
    /// Reads and formats the number once the user leaves the text box.
    /// </summary>
    private async Task OnTextBlur()
    {
        await this.CommitAsync();
    }

    /// <summary>
    /// Builds the E.164 value from the region and the text, shows the number in the region's format, and raises
    /// <see cref="ValueChanged"/> when the value changed. A number typed with its calling code selects its region.
    /// </summary>
    private async Task CommitAsync()
    {
        var text = this.Text?.Trim() ?? string.Empty;
        string value;

        if (text.Length == 0)
        {
            value = string.Empty;
        }
        else if (TryParse(text, this.RegionCode, out var number))
        {
            this.RegionCode = RegionOf(number, this.RegionCode);
            value = PhoneNumberUtil.Format(number, PhoneNumberFormat.E164);
            this.Text = PhoneNumberUtil.IsValidNumber(number)
                ? PhoneNumberUtil.Format(number, PhoneNumberFormat.NATIONAL)
                : text;
        }
        else
        {
            value = text;
        }

        if (value == (this.Value ?? string.Empty))
        {
            return;
        }

        this.Value = value;
        this.lastValue = value;
        await this.ValueChanged.InvokeAsync(value);

        if (this.EditContext != null && this.ValueExpression != null)
        {
            this.EditContext.NotifyFieldChanged(FieldIdentifier.Create(this.ValueExpression));
        }
    }
}
