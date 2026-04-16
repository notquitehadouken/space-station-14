using Content.Client.CombatMode;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Utility;

namespace Content.Client.Weapons.Melee;

public sealed class MeleeParrySystem : SharedMeleeParrySystem
{
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly SpriteSystem _spriteSys = default!;
    private Texture? _parrySight;

    public override void Initialize()
    {
        base.Initialize();

        _parrySight = _spriteSys.Frame0(new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/crosshair_pointers.rsi"),
            "melee_parry_sight"));

        SubscribeAllEvent<QueryCombatReticleEvent>(OnCombatReticleQuery);
    }

    public void OnCombatReticleQuery(QueryCombatReticleEvent args)
    {
        if (_playerMan.LocalEntity is { } uid)
        {
            if (HasComp<MeleeParryingComponent>(uid))
            {
                args.Texture = _parrySight;
            }
        }
    }
}
