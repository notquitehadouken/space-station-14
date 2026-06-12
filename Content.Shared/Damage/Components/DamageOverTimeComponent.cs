namespace Content.Shared.Damage.Components;

/// <summary>
/// Applies damage over time to any mob with damageable and this component.
/// </summary>
[RegisterComponent]
public sealed partial class DamageOverTimeComponent : Component
{
    /// <summary>
    /// How much damage will be applied next.
    /// </summary>
    [DataField]
    public DamageSpecifier CurrentDamage;

    /// <summary>
    /// After damage is applied, add this to <see cref="CurrentDamage"/>.
    /// </summary>
    [DataField]
    public DamageSpecifier? DamageIncrease;

    /// <summary>
    /// How long between damage applications.
    /// </summary>
    [DataField]
    public TimeSpan Interval = TimeSpan.FromSeconds(1);

    /// <summary>
    /// If, when applying damage, multiply the value by <see cref="Interval"/> in seconds.
    /// Just in case you want to say <see cref="CurrentDamage"/> is damage per second and not damage per application.
    /// </summary>
    /// <remarks>
    /// No effect when <see cref="Interval"/> is 1 second.
    /// </remarks>
    [DataField]
    public bool MultiplyByInterval = false;

    /// <summary>
    /// If <see cref="CurrentDamage"/> will heal when any damage value is negative.
    /// When false, damage types in <see cref="CurrentDamage"/> are given a minimum of 0.
    /// </summary>
    [DataField]
    public bool CanHeal = false;

    /// <summary>
    /// Does damage ignore resistances?
    /// </summary>
    [DataField]
    public bool IgnoreResistances = true;

    /// <summary>
    /// Last time damage was applied.
    /// </summary>
    [DataField]
    public TimeSpan LastApplication;
}
