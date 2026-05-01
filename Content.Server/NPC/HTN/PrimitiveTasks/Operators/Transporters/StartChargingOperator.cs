using Content.Server.Power.EntitySystems;
using Content.Server.Transporters;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Transporters;

/// <summary>
/// Attempt to charge the transporter until it is at full charge.
/// </summary>
public sealed partial class StartChargingOperator : HTNOperator, IHtnConditionalShutdown
{
    [Dependency] private readonly TransporterSystem _transporterSys = default!;
    [DataField] public string Key = "Target";

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var target = blackboard.GetValue<EntityUid>(Key);

        var result = _transporterSys.TryCharge(owner, target, frameTime);

        return result ? HTNOperatorStatus.Continuing : HTNOperatorStatus.Finished;
    }

    public HTNPlanState ShutdownState { get; } = HTNPlanState.TaskFinished;
    public void ConditionalShutdown(NPCBlackboard blackboard)
    {

    }
}
