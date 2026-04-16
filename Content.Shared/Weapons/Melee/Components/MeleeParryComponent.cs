using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.Components;

/// <summary>
/// For melee weapons capable of parrying.
/// Parrying negates all damage to the wielder.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class MeleeParryComponent : Component
{
    /// <summary>
    /// If you are allowed to attack during your parry attempt.
    /// Attacking while parrying will always cancel the parry.
    /// </summary>
    [DataField]
    public bool AllowAttackingWhileParrying = false;

    /// <summary>
    /// If there are no parries in an attempt, this is how long before you can make another attempt.
    /// Measured from the end of the parry window.
    /// </summary>
    [DataField]
    public TimeSpan FailureCooldown = TimeSpan.FromSeconds(3.0);

    /// <summary>
    /// If there is a successful parry in an attempt, this is how long before you can make another attempt.
    /// Measured from the end of the last reparry window.
    /// </summary>
    [DataField]
    public TimeSpan SuccessCooldown = TimeSpan.Zero;

    /// <summary>
    /// On a parry attempt, this is the amount of time before the attempt is considered unsuccessful if no parry occurs.
    /// </summary>
    [DataField]
    public TimeSpan ParryWindow = TimeSpan.FromSeconds(1.0);

    /// <summary>
    /// On a successful parry, this is how long the window for another parry (a reparry) to occur is.
    /// </summary>
    [DataField]
    public TimeSpan ReparryWindow = TimeSpan.FromSeconds(0.35);

    /// <summary>
    /// Last time a parry was attempted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan LastParry;

    [DataField, AutoNetworkedField]
    public bool NextParryWillReparry = false;

    [DataField, AutoNetworkedField]
    public bool LastParryWasSuccess = false;

    /// <summary>
    /// After this parry ends, how long until another parry can be attempted.
    /// </summary>
    public TimeSpan Cooldown => LastParryWasSuccess ? SuccessCooldown : FailureCooldown;

    public TimeSpan Window => NextParryWillReparry ? ReparryWindow : ParryWindow;

    /// <summary>
    /// When this parry should end.
    /// </summary>
    public TimeSpan ParryEnd
    {
        get => LastParry + Window;
        set => LastParry = value - Window;
    }

    /// <summary>
    /// When the user can attempt another parry.
    /// </summary>
    public TimeSpan NextParry
    {
        get => ParryEnd + Cooldown;
        set => ParryEnd = value - Cooldown;
    }

    /// <summary>
    /// If parried bullets or lasers are reflected instead of absorbed.
    /// </summary>
    [DataField]
    public bool ReflectRanged = false;

    /// <summary>
    /// If parried melee strikes are reflected instead of absorbed.
    /// Reflected melee strikes cannot be parried.
    /// </summary>
    [DataField]
    public bool ReflectMelee = false;

    /// <summary>
    /// If a melee strike is parried, this is how long the attacker is unable to perform another melee strike.
    /// </summary>
    [DataField]
    public TimeSpan ParriedMeleeStunTime = TimeSpan.FromSeconds(1.5);

    /// <summary>
    /// The sound to play when a parry happens.
    /// </summary>
    [DataField]
    public SoundSpecifier ParrySound = new SoundPathSpecifier("/Audio/Weapons/block_metal1.ogg", AudioParams.Default.WithVariation(0.05f));

    /// <summary>
    /// The sound to play when you attempt to parry.
    /// </summary>
    [DataField]
    public SoundSpecifier ParryAttemptSound = new SoundPathSpecifier("/Audio/Weapons/ding.ogg");
}
