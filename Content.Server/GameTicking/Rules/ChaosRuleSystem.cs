using Content.Server.Chat.Managers;
using Content.Server.GameTicking.Rules.Configurations;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Components;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules;

public sealed class ChaosRuleSystem : GameRuleSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ShuttleSystem _shuttle = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    public override string Prototype => "Chaos";

    public void OnStartAttempt(RoundStartAttemptEvent ev)
    {
        if (!RuleAdded)
            return;

        var minPlayers = _cfg.GetCVar(CCVars.ChaosMinPlayers);
        if (!ev.Forced && ev.Players.Length < minPlayers)
        {
            _chatManager.DispatchServerAnnouncement(Loc.GetString("nukeops-not-enough-ready-players", ("readyPlayersCount", ev.Players.Length), ("minimumPlayers", minPlayers)));
            ev.Cancel();
            return;
        }

        if (ev.Players.Length == 0)
        {
            _chatManager.DispatchServerAnnouncement(Loc.GetString("nukeops-no-one-ready"));
            ev.Cancel();
            return;
        }
    }

    public override void Started()
    {
        var ticker = EntitySystem.Get<GameTicker>();
        String[] gamerules =
        {
            // Main antags (no suspicion, sorry suspicion)
            "Traitor",
            "Pirates",
            "Nukeops",
            "Zombie",

            // Events
            "BreakerFlip",
            "BureaucraticError",
            "DiseaseOutbreak",
            "Dragon",
            "FalseAlarm",
            "GasLeak",
            "KudzuGrowth",
            "MeteorSwarm",
            "MouseMigration",
            "PowerGridCheck",
            "RandomSentience",
            "VentClog",
            "VentCritters"
        };
        // probably shouldnt hardcode this but whatever
        foreach (var ruleId in gamerules)
        {
            if (!_prototypeManager.TryIndex<GameRulePrototype>(ruleId, out var rule))
                continue;

            ticker.AddGameRule(rule);
        }

        foreach (var comp in EntityQuery<StationDataComponent>(true))
        {
            _shuttle.CallEmergencyShuttle(comp.Owner);
        }
    }

    public override void Ended() {}
}
