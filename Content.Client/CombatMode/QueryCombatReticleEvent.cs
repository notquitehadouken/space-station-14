using Robust.Client.Graphics;

namespace Content.Client.CombatMode;

/// <summary>
/// If Texture is changed to non-null, use that texture as a reticle instead of what would normally be used.
/// </summary>
[Serializable]
public sealed class QueryCombatReticleEvent : EntityEventArgs
{
    /// <summary>
    /// The texture.
    /// </summary>
    public Texture? Texture;
}
