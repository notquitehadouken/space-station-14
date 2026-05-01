namespace Content.Server.Transporters.Components;

[RegisterComponent]
public sealed partial class TransporterComponent : Component
{
    /// <summary>
    /// The wattage that this transporter uses to run.
    /// Not consumed while idling.
    /// As a note, small-cap cells have 360 joules, medium have 720, and high-cap have 1080
    /// </summary>
    [DataField]
    public float DrainWattage = 3;

    /// <summary>
    /// The joules at which the transporter decides it is on low charge.
    /// </summary>
    [DataField]
    public float LowWattageThreshold = 3 * 20;

    /// <summary>
    /// If we will try to charge at the target reciever after a delivery.
    /// </summary>
    [DataField]
    public bool ChargeAfterDelivery = true;

    /// <summary>
    /// The item this transporter is trying to pick up.
    /// </summary>
    [DataField]
    public EntityUid? TargetItem;

    /// <summary>
    /// If the transporter is charging, this is the charger its using.
    /// </summary>
    [DataField]
    public EntityUid? ChargingFrom;
}
