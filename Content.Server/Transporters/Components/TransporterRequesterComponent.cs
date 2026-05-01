namespace Content.Server.Transporters.Components;

[RegisterComponent]
public sealed partial class TransporterRequesterComponent : Component
{
    /// <summary>
    /// The id used for this requester.
    /// It will request items from any provider with the same id.
    /// </summary>
    [DataField]
    public string Id = "";

    /// <summary>
    /// Transporters that are on their way to drop off an item.
    /// </summary>
    [DataField]
    public List<EntityUid> TransportersEnroute = new();
}
