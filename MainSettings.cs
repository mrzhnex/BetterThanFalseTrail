using Exiled.API.Features;

namespace BetterThanFalseTrail
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(BetterThanFalseTrail);
        public SetEvents SetEvents { get; set; }

        public override void OnEnabled()
        {
            SetEvents = new SetEvents();
            Exiled.Events.Handlers.Player.InsertingGeneratorTablet += SetEvents.OnInsertingGeneratorTablet;
            Exiled.Events.Handlers.Player.PickingUpItem += SetEvents.OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingItem += SetEvents.OnChangingItem;
            Exiled.Events.Handlers.Player.UsingMedicalItem += SetEvents.OnUsingMedicalItem;
            Exiled.Events.Handlers.Player.MedicalItemUsed += SetEvents.OnMedicalItemUsed;
            Exiled.Events.Handlers.Player.StoppingMedicalItem += SetEvents.OnStoppingMedicalItem;
            Exiled.Events.Handlers.Player.Spawning += SetEvents.OnSpawning;
            Exiled.Events.Handlers.Server.WaitingForPlayers += SetEvents.OnWaitingsForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += SetEvents.OnSendingConsoleCommand;
            Exiled.Events.Handlers.Server.RespawningTeam += SetEvents.OnRespawningTeam;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += SetEvents.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Player.ChangingRole += SetEvents.OnChangingRole;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.InsertingGeneratorTablet -= SetEvents.OnInsertingGeneratorTablet;
            Exiled.Events.Handlers.Player.PickingUpItem -= SetEvents.OnPickingUpItem;
            Exiled.Events.Handlers.Player.ChangingItem -= SetEvents.OnChangingItem;
            Exiled.Events.Handlers.Player.UsingMedicalItem -= SetEvents.OnUsingMedicalItem;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= SetEvents.OnMedicalItemUsed;
            Exiled.Events.Handlers.Player.StoppingMedicalItem -= SetEvents.OnStoppingMedicalItem;
            Exiled.Events.Handlers.Player.Spawning -= SetEvents.OnSpawning;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= SetEvents.OnWaitingsForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= SetEvents.OnSendingConsoleCommand;
            Exiled.Events.Handlers.Server.RespawningTeam -= SetEvents.OnRespawningTeam;
            Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= SetEvents.OnSendingRemoteAdminCommand;
            Exiled.Events.Handlers.Player.ChangingRole -= SetEvents.OnChangingRole;
            Log.Info(Name + " off");
        }
    }
}