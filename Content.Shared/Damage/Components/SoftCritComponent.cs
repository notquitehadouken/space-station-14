using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components;

/// <summary>
///     A mob will have this component if it has passed the threshold for critical
///     This means even if they have died, they will still have this component
///     This should not be present in the definition of any entity
/// </summary>
[RegisterComponent]
public sealed partial class SoftCritComponent : Component
{
    /// <summary>
    ///     When we entered softcrit
    /// </summary>
    [DataField]
    public TimeSpan SoftCritEntered = TimeSpan.Zero;

    /// <summary>
    ///     How long softcrit lasts
    /// </summary>
    [DataField]
    public TimeSpan SoftCritTime = TimeSpan.FromSeconds(5);

    /// <summary>
    ///     At what time softcrit will end
    /// </summary>
    public TimeSpan SoftCritEnds => SoftCritEntered + SoftCritTime;

    /// <summary>
    ///     If softcrit has ended yet
    /// </summary>
    [DataField]
    public bool SoftCritEnded = false;
}
