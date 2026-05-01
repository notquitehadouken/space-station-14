using Content.Server.Transporters.Components;

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Checks if any providers with items exist.
/// </summary>
public sealed partial class AvailableProviderPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField("invert")]
    public bool Invert = false;

    public override bool IsMet(NPCBlackboard blackboard)
    {
        var query = _entManager.EntityQueryEnumerator<TransporterProviderComponent>();

        while (query.MoveNext(out var comp))
        {
            if (comp.AnyToSend)
            {
                return !Invert;
            }
        }

        return Invert;
    }
}
