using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits;

/// <summary>
/// Describes a trait.
/// </summary>
[Prototype]
public sealed partial class TraitPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The name of this trait.
    /// </summary>
    [DataField]
    public LocId Name { get; private set; } = string.Empty;

    /// <summary>
    /// The description of this trait.
    /// </summary>
    [DataField]
    public LocId? Description { get; private set; }

    /// <summary>
    /// Don't apply this trait to entities this whitelist IS NOT valid for.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Don't apply this trait to entities this whitelist IS valid for. (hence, a blacklist)
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// The components that get added to the player, when they pick this trait.
    /// </summary>
    [DataField]
    public ComponentRegistry Components { get; private set; } = default!;

    /// <summary>
    /// Gear that is given to the player, when they pick this trait.
    /// </summary>
    [DataField]
    public EntProtoId? TraitGear;

    /// <summary>
    /// Trait Price. If negative number, points will be added.
    /// </summary>
    [DataField]
    public int Cost = 0;

    /// <summary>
    /// If DefaultCategory is null, then ExclusiveCategory will stand in for it.
    /// Always set to DefaultCategory.
    /// </summary>
    public ProtoId<TraitCategoryPrototype>? Category
    {
        get => DefaultCategory ?? ExclusiveCategory;
        set => DefaultCategory = value;
    }

    /// <summary>
    /// Adds a trait to a category, allowing you to limit the selection of some traits to the settings of that category.
    /// </summary>
    [DataField("category")]
    public ProtoId<TraitCategoryPrototype>? DefaultCategory;

    /// <summary>
    /// If set, then this trait can only be selected if no other traits in that category are, and if this trait
    /// is selected then no other traits in that category can be selected
    /// </summary>
    [DataField]
    public ProtoId<TraitCategoryPrototype>? ExclusiveCategory;
}
