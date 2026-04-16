namespace Content.Shared.Weapons.Melee.Events;

/// <summary>
/// Raised directed on the weapon when a parry ends.
/// </summary>
[ByRefEvent]
public record struct ParryResultEvent(EntityUid User, bool ParrySucceeded);
