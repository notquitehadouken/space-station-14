using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components;

/// <summary>
///     A mob must have this component to enter softcrit
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SoftCritCapableComponent : Component
{
    /// <summary>
    ///     How long softcrit lasts
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan SoftCritTime = TimeSpan.FromSeconds(5);
}
