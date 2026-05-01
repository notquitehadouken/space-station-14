using Content.Shared.Power.Components;

namespace Content.Server.Transporters.Components;

/// <summary>
/// Any machine capable of charging the batteries in transporters.
/// </summary>
[RegisterComponent]
public sealed partial class TransporterChargerComponent : Component
{
    /// <summary>
    /// How fast a charger can charge an individual transporter
    /// </summary>
    [DataField]
    public float Wattage = 60;

    /// <summary>
    /// The maximum amount of transporters that can use this charger for charging at the same time.
    /// </summary>
    [DataField]
    public int MaxTransporters = 2;

    /// <summary>
    /// List of transporters currently charging.
    /// </summary>
    [DataField]
    public List<EntityUid> Charging = new();

    /// <summary>
    /// If a bot can start charging at this charger.
    /// </summary>
    public bool SpotsAvailable => Charging.Count < MaxTransporters;

    /// <summary>
    /// Transporters trying to charge at this charger.
    /// </summary>
    [DataField]
    public Queue<EntityUid> WaitingTransporters = new();
}
