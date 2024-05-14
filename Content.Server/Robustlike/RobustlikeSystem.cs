using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Content.Server.GameTicking;
using Robust.Shared.Map;

namespace Content.Server.Robustlike;

public sealed class RobustlikeSystem : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawn);
    }

    private void TrySendToNewMap(EntityUid uid, out MapId mapId)
    {
        var map = _mapSystem.CreateMap();
        var xForm = Transform(uid);
        mapId = Transform(map).MapID;
        _transform.SetMapCoordinates(uid, new MapCoordinates(Vector2.Zero, mapId));
    }

    private void OnPlayerSpawn(PlayerSpawnCompleteEvent args)
    {
        TrySendToNewMap(args.Mob, out MapId mapId);
    }
}
