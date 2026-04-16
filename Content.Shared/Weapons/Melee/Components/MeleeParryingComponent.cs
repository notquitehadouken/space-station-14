using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class MeleeParryingComponent : Component
{
    /// <summary>
    /// The weapon used to parry.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid Weapon;
}
