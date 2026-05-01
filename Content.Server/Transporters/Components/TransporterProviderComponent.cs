namespace Content.Server.Transporters.Components;

[RegisterComponent]
public sealed partial class TransporterProviderComponent : Component
{
    /// <summary>
    /// The id used for this provider.
    /// It will send items to any requester with the same id.
    /// </summary>
    [DataField]
    public string Id = "";

    /// <summary>
    /// Items this provider is trying to send out.
    /// </summary>
    [DataField]
    public Queue<EntityUid> ToSend = new();

    /// <summary>
    /// Transporters that are on their way to pick up an item.
    /// </summary>
    [DataField]
    public List<EntityUid> TransportersEnroute = new();

    [DataField]
    public bool TrickOrTreat = false;

    public bool AnyToSend => TrickOrTreat || ToSend.Count > TransportersEnroute.Count;
}
