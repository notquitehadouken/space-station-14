using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components;

/// <summary>
///     Like ActiveStaminaComponent, but different.
///     Used for softcrit.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ActiveDamageComponent : Component
{

}
