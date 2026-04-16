using System.Diagnostics;
using Content.Shared.Audio;
using Content.Shared.Damage;
using Content.Shared.Damage.Systems;
using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Projectiles;
using Content.Shared.Stunnable;
using Content.Shared.Timing;
using Content.Shared.Weapons.Hitscan.Events;
using Content.Shared.Weapons.Melee.Components;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Reflect;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Shared.Weapons.Melee;

public abstract class SharedMeleeParrySystem : EntitySystem
{
    public static readonly string ParryDelayName = "parry";
    public static readonly string ParryPopup = "parry-popup";

    [Dependency] protected readonly IGameTiming Timing = default!;
    [Dependency] private readonly UseDelaySystem _useDelaySys = default!;
    [Dependency] protected readonly SharedAudioSystem AudioSys = default!;
    [Dependency] private readonly SharedPopupSystem _popupSys = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MeleeParryComponent, DroppedEvent>(OnDrop);
        SubscribeLocalEvent<MeleeParryComponent, ItemUnwieldedEvent>(OnUnwield);
        SubscribeLocalEvent<MeleeParryComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<MeleeParryComponent, AttemptMeleeEvent>(OnMeleeAttempt);
        SubscribeLocalEvent<MeleeParryComponent, HitScanReflectAttemptEvent>(OnHitscanReflect, after: [typeof(ReflectSystem)]);
        SubscribeLocalEvent<MeleeParryComponent, AttemptHitscanRaycastFiredEvent>(OnHitscanHit, before: [typeof(ReflectSystem)]);
        SubscribeLocalEvent<MeleeParryComponent, ProjectileReflectAttemptEvent>(OnProjectileReflect, after: [typeof(ReflectSystem)]);
        SubscribeLocalEvent<MeleeParryingComponent, ProjectileHitEvent>(OnProjectileHit);
    }

    private void OnMapInit(Entity<MeleeParryComponent> uid, ref MapInitEvent args)
    {
        uid.Comp.NextParry = Timing.CurTime;
    }

    /// <summary>
    /// Called to end a parry.
    /// </summary>
    private void EndParry(EntityUid user, Entity<MeleeParryComponent> uid)
    {
        var ev = new ParryResultEvent(user, uid.Comp.LastParryWasSuccess);

        _useDelaySys.SetLength(uid.Owner, uid.Comp.Cooldown, id: ParryDelayName);
        _useDelaySys.TryResetDelay(uid.Owner, id: ParryDelayName);

        RaiseLocalEvent(uid, ref ev);

        RemComp<ReflectComponent>(uid);
        RemComp<MeleeParryingComponent>(user);

        uid.Comp.ParryEnd = Timing.CurTime;
    }

    /// <summary>
    /// If the user is in a valid position to parry.
    /// This does not include cooldowns.
    /// </summary>
    public bool ParryValidNow(EntityUid user, Entity<MeleeParryComponent> uid)
    {
        if (TryComp(uid, out WieldableComponent? wieldable)
            && !wieldable.Wielded)
        {
            return false;
        }

        if (HasComp<KnockedDownComponent>(user))
        {
            return false; // YOU try to parry something while on the floor.
        }

        return true;
    }

    private void OnParry(Entity<MeleeParryComponent> uid, bool silent = true)
    {
        uid.Comp.LastParry = Timing.CurTime;
        uid.Comp.NextParryWillReparry = true;
        uid.Comp.LastParryWasSuccess = true;

        if (!silent)
        {
            AudioSys.PlayPvs(uid.Comp.ParrySound, uid);
            _popupSys.PopupClient(ParryPopup, null);
        }
    }

    private void OnHitscanReflect(Entity<MeleeParryComponent> uid, ref HitScanReflectAttemptEvent args)
    {
        if (args.Reflected)
        {
            OnParry(uid);
        }
    }

    private void OnHitscanHit(Entity<MeleeParryComponent> uid, ref AttemptHitscanRaycastFiredEvent args)
    {
        if (args.Cancelled)
        {
            return;
        }

        if (uid.Comp.ReflectRanged)
        {
            return;
        }

        OnParry(uid, false);

        args.Data.HitEntity = null; // Nuh uh, you didn't hit me!
    }

    private void OnProjectileReflect(Entity<MeleeParryComponent> uid, ref ProjectileReflectAttemptEvent args)
    {
        if (args.Cancelled)
        {
            OnParry(uid);
        }
    }

    private void OnProjectileHit(Entity<MeleeParryingComponent> uid, ref ProjectileHitEvent args)
    {
        Log.Debug("projectile hit");
        var weapon = uid.Comp.Weapon;
        if (!TryComp<MeleeParryComponent>(weapon, out var parryComp))
        {
            return;
        }

        OnParry((weapon, parryComp), false);

        Log.Debug("set damage to 0");
        args.Damage = new DamageSpecifier();
    }

    private void OnMeleeAttempt(Entity<MeleeParryComponent> uid, ref AttemptMeleeEvent args)
    {
        if (!HasComp<MeleeParryingComponent>(args.User))
        {
            return;
        }

        if (Timing.CurTime <= uid.Comp.ParryEnd)
        {
            if (uid.Comp.AllowAttackingWhileParrying)
            {
                EndParry(args.User, uid);
            }
            else
            {
                args.Cancelled = true;
                args.Message = null;
            }
        }
    }

    private void OnDrop(Entity<MeleeParryComponent> uid, ref DroppedEvent args)
    {
        // You have been diagnosed with failure
        if (HasComp<MeleeParryingComponent>(args.User))
        {
            uid.Comp.LastParryWasSuccess = false;
            EndParry(args.User, uid);
        }
    }

    private void OnUnwield(Entity<MeleeParryComponent> uid, ref ItemUnwieldedEvent args)
    {
        if (HasComp<MeleeParryingComponent>(args.User))
        {
            uid.Comp.LastParryWasSuccess = false;
            EndParry(args.User, uid);
        }
    }

    public override void Update(float deltaTime)
    {
        var query = EntityQueryEnumerator<MeleeParryingComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            var weapon = comp.Weapon;
            if (!TryComp<MeleeParryComponent>(weapon, out var meleeParry))
            {
                continue;
            }

            if (Timing.CurTime >= meleeParry.ParryEnd)
            {
                EndParry(uid, (weapon, meleeParry));
            }
        }
    }
}
