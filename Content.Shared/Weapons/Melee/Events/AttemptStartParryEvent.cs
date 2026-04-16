namespace Content.Shared.Weapons.Melee.Events;

/// <summary>
/// Raised directed on a weapon which someone is trying to parry with.
/// </summary>
[ByRefEvent]
public record struct AttemptStartParryEvent(EntityUid User, bool Cancelled = false);
