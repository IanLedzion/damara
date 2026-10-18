namespace Damara.Fixture.Widget.Capabilities.Gear;

/// <summary>
/// Hand-written half of the Gear capability: implement the
/// On*Changing / On*Changed partial methods that carry property-update
/// cascades (the same convention as the Devart entity hooks), and the
/// OnWidgetCreated / OnWidgetPropertyChanging /
/// OnWidgetPropertyChanged partial methods that carry hand-coded
/// initialisation and reactions to writes on the underlying entity (e.g.
/// reinitialising properties when a defining property changes), and
/// OnWidgetGearPropertyChanging / OnWidgetGearPropertyChanged for writes on
/// the WidgetGear row backing the capability.
/// Unimplemented handlers are elided. No unit-of-work access here - cascades
/// needing lookups escalate to a domain service.
/// </summary>
public sealed partial class GearCapability
{
}
