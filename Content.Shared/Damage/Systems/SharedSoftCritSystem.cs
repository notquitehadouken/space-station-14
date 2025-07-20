using Content.Shared.Damage.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Damage.Systems;

public sealed class SharedSoftCritSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SoftCritComponent, DamageChangedEvent>(OnDamageChanged);
    }

    public override void Update(float deltaTime)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        var query = EntityQueryEnumerator<SoftCritComponent>();

        while (query.MoveNext(out var uid, out var softcrit))
        {
            if (_timing.CurTime < softcrit.SoftCritEnds)
                continue;

            if (softcrit.SoftCritEnded)
                continue;

            softcrit.SoftCritEnded = true;

            UpdateSoftCritStatus(uid);
            _mobThreshold.VerifyThresholds(uid); // Prognosis? HA ha, YOU'RE DEAD!
        }
    }

    public void OnDamageChanged(Entity<SoftCritComponent> uid, ref DamageChangedEvent args)
        => UpdateSoftCritStatus(uid, null, args.Damageable);

    public void UpdateSoftCritStatus(EntityUid uid, MobStateComponent? mobState = null, DamageableComponent? damageable = null)
    {
        if (!Resolve(uid, ref mobState, ref damageable))
            return;

        if (!TryComp(uid, out SoftCritCapableComponent? capable))
            return;

        if(!_mobThreshold.TryGetIncapThreshold(uid, out var incapThreshold) ||
           !_mobThreshold.TryGetDeadThreshold(uid, out var deadThreshold))
            return;

        if (damageable.TotalDamage >= deadThreshold)
        {
            // Ensure we die immediately
            var soft = EnsureComp<SoftCritComponent>(uid);
            soft.SoftCritEntered = _timing.CurTime - capable.SoftCritTime - TimeSpan.FromSeconds(1);
            soft.SoftCritTime = capable.SoftCritTime;
            soft.SoftCritEnded = true;
            return;
        }
        else if (damageable.TotalDamage < incapThreshold)
        {
            RemComp<SoftCritComponent>(uid);
            return;
        }

        if (HasComp<SoftCritComponent>(uid))
            return;

        EnsureComp(uid, out SoftCritComponent softCrit);

        softCrit.SoftCritEntered = _timing.CurTime;
        softCrit.SoftCritTime = capable.SoftCritTime;
    }

    public bool CanIncapNow(EntityUid uid, MobStateComponent? mobState = null)
    {
        if (!Resolve(uid, ref mobState))
            return false; // wtf are you doing

        UpdateSoftCritStatus(uid, mobState);

        if (!TryComp(uid, out SoftCritCapableComponent? capable)
            || !TryComp(uid, out SoftCritComponent? softCrit))
            return true;

        if (softCrit.SoftCritEnds < _timing.CurTime)
            return true;

        return false;
    }
}
