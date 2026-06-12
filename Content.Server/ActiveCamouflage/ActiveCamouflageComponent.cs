using Content.Shared.Damage;

namespace Content.Server.ActiveCamouflage;

[RegisterComponent]
public sealed partial class ActiveCamouflageComponent : Component
{
    /// <summary>
    /// Cooldown until you can turn off the implant.
    /// </summary>
    [DataField]
    public TimeSpan DeactivateCooldown = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Cooldown until you can turn on the implant.
    /// </summary>
    [DataField]
    public TimeSpan ReactivateCooldown = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Are we currently camouflaged?
    /// </summary>
    [DataField]
    public bool Enabled = false;

    /// <summary>
    /// How long you can keep the implant active before you start taking damage.
    /// </summary>
    [DataField]
    public TimeSpan SafetyPeriod = TimeSpan.FromSeconds(10);

    /// <summary>
    /// What kind of damage is applied.
    /// This damage accumulates over time.
    /// </summary>
    [DataField]
    public DamageSpecifier Damages;
}
