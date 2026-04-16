using Content.Shared.Interaction.Events;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Components;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Reflect;
using Content.Shared.Wieldable;

namespace Content.Server.Weapons.Melee;

public sealed class MeleeParrySystem : SharedMeleeParrySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MeleeParryComponent, UseInHandEvent>(OnUseInHand, before: [typeof(SharedWieldableSystem)]);
    }

    private bool TryParry(EntityUid user, Entity<MeleeParryComponent> uid)
    {
        if (Timing.CurTime <= uid.Comp.NextParry)
        {
            return false;
        }

        var ev = new AttemptStartParryEvent(user);

        RaiseLocalEvent(uid, ref ev);

        if (ev.Cancelled)
        {
            return false;
        }

        AudioSys.PlayPvs(uid.Comp.ParryAttemptSound, user);

        uid.Comp.LastParry = Timing.CurTime;
        uid.Comp.NextParryWillReparry = false;
        uid.Comp.LastParryWasSuccess = false;

        EnsureComp<MeleeParryingComponent>(user, out var parrying);
        parrying.Weapon = uid;

        if (uid.Comp.ReflectRanged)
        {
            EnsureComp<ReflectComponent>(uid, out var reflect);
            reflect.ReflectProb = 1.0f;
            reflect.InRightPlace = true;
            reflect.HideExamine = true;
            reflect.ReflectPopup = ParryPopup;
            reflect.SoundOnReflect = uid.Comp.ParrySound;
        }

        return true;
    }

    private void OnUseInHand(Entity<MeleeParryComponent> uid, ref UseInHandEvent args)
    {
        if (!ParryValidNow(args.User, uid))
        {
            return;
        }

        args.Handled = true;

        if (!TryParry(args.User, uid))
        {
            args.ApplyDelay = false;
        }
    }
}
