using Robust.Shared.Prototypes;

namespace Content.Shared.Traits;

public abstract class SharedTraitSystem : EntitySystem
{
    [Dependency] protected readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();
    }

    public bool HasTrait(EntityUid mob, ProtoId<TraitPrototype> trait)
    {
        if (!TryComp<TraitsComponent>(mob, out var traits) ||
            !_prototypeManager.TryIndex(trait, out var traitProto) ||
            !traits.Traits.Contains(traitProto))
            return false;

        return true;
    }
}
