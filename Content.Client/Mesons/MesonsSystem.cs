using Robust.Client.Graphics;
using Robust.Client.Player;
using Content.Shared.Eye.Blinding.Components;
using Content.Shared.GameTicking;
using Robust.Shared.Player;

namespace Content.Client.Mesons;

public sealed class MesonsSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private MesonsOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new();
        _overlayMan.AddOverlay(_overlay);
    }
}