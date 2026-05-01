using Content.Server.Power.EntitySystems;
using Content.Server.Transporters.Components;

namespace Content.Server.Transporters;

/// <summary>
/// Entity system handling transporters.
/// </summary>
public sealed class TransporterSystem : EntitySystem
{
    [Dependency] private readonly BatterySystem _batterySys = default!;

    public override void Initialize()
    {
        base.Initialize();
    }

    public bool TryCharge(EntityUid transporter, EntityUid charger, float deltaTime)
    {
        if (_batterySys.IsFull(transporter))
        {
            return false;
        }

        if (!TryComp<TransporterChargerComponent>(charger, out var chargerComponent))
        {
            return false;
        }

        if (chargerComponent.Charging.Contains(transporter))
        {
            var deltaCharge = _batterySys.ChangeCharge(transporter, chargerComponent.Wattage * deltaTime);
            if (deltaCharge == 0)
            {
                chargerComponent.Charging.Remove(transporter);
                return false;
            }

            return true;
        }

        if (!chargerComponent.WaitingTransporters.Contains(transporter))
        {
            chargerComponent.WaitingTransporters.Enqueue(transporter);
            return true;
        }

        if (!chargerComponent.SpotsAvailable)
        {
            return true;
        }

        if (chargerComponent.WaitingTransporters.TryPeek(out var first) && first == transporter)
        {
            chargerComponent.WaitingTransporters.Dequeue();
            chargerComponent.Charging.Add(transporter);
            return true;
        }

        return false;
    }
}
