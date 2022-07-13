using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Ghost;
using Robust.Shared.Utility;

namespace Content.Server.Ghost.Components
{
    [RegisterComponent]
    [ComponentReference(typeof(SharedGhostComponent))]
    public sealed class GhostComponent : SharedGhostComponent
    {
        public TimeSpan TimeOfDeath { get; set; } = TimeSpan.Zero;

        [DataField("booRadius")]
        public float BooRadius = 3;

        [DataField("booMaxTargets")]
        public int BooMaxTargets = 3;

        [DataField("booaction")]
        public InstantAction BooAction = new()
        {
            UseDelay = TimeSpan.FromSeconds(120),
            Icon = new SpriteSpecifier.Texture(new ResourcePath("Interface/Actions/scream.png")),
            Name = "action-name-boo",
            Description = "action-description-boo",
            CheckCanInteract = false,
            Event = new BooActionEvent(),
        };

        [DataField("resaction")]
        public InstantAction ResAction = new()
        {
            UseDelay = TimeSpan.FromSeconds(60),
            Icon = new SpriteSpecifier.Texture(new ResourcePath("Interface/Actions/break_realspace.png")),
            Name = "action-name-respawn",
            Description = "action-description-respawn",
            CheckCanInteract = false,
            Event = new ResActionEvent(),
        };
    }

    public sealed class BooActionEvent : InstantActionEvent { }

    public sealed class ResActionEvent : InstantActionEvent { }
}
