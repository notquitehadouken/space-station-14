using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TraitsComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<ProtoId<TraitPrototype>> Traits;
}
