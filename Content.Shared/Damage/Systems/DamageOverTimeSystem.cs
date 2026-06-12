using Content.Shared.Damage.Components;
using Robust.Shared.Timing;

namespace Content.Shared.Damage.Systems;

public sealed partial class DamageOverTimeSystem : EntitySystem
{
    [Dependency] private DamageableSystem _damageSys = default!;
    [Dependency] private IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageOverTimeComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<DamageOverTimeComponent> uid, ref MapInitEvent args)
    {
        uid.Comp.LastApplication = _timing.CurTime;
    }

    private void ApplyDamage(Entity<DamageableComponent?> uid, DamageOverTimeComponent dotcomp)
    {
        var specifier = dotcomp.CurrentDamage;

        if (!dotcomp.CanHeal)
        {
            specifier = DamageSpecifier.GetPositive(specifier);
        }

        if (!specifier.Empty)
        {
            if (dotcomp.MultiplyByInterval)
            {
                specifier *= dotcomp.Interval.TotalSeconds;
            }

            _damageSys.TryChangeDamage(uid,
                specifier,
                ignoreResistances: dotcomp.IgnoreResistances,
                interruptsDoAfters: false);
        }

        if (dotcomp.DamageIncrease is { } damageIncrease)
        {
            dotcomp.CurrentDamage += damageIncrease;
        }
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<DamageableComponent, DamageOverTimeComponent>();

        while (query.MoveNext(out var uid, out var damageablecomp, out var dotcomp))
        {
            if (dotcomp.LastApplication + dotcomp.Interval <= _timing.CurTime)
                continue;

            dotcomp.LastApplication += dotcomp.Interval;
            ApplyDamage(uid, dotcomp);
        }
    }
}
