using Exiled.API.Features;
using Exiled.Events.EventArgs;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterThanFalseTrail
{
    public class SetEvents
    {
        internal void OnMedicalItemUsed(UsedMedicalItemEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<UseMedicalItemComponent>());
            }
        }

        internal void OnUsingMedicalItem(UsingMedicalItemEventArgs ev)
        {
            if (ev.Item != ItemType.SCP500)
            {
                if (ev.Player.GameObject.GetComponent<UseMedicalItemComponent>())
                {
                    UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<UseMedicalItemComponent>());
                }
                UseMedicalItemComponent useMedicalItemComponent = ev.Player.GameObject.AddComponent<UseMedicalItemComponent>();
                useMedicalItemComponent.ItemType = ev.Item;
            }
        }

        internal void OnRoundStarted()
        {
            GameObject.FindWithTag("FemurBreaker").AddComponent<SpawnWithWhitelistComponent>();
        }

        internal void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                Global.PlayersVotes = new Dictionary<int, int>();
                foreach (Player player in ev.Players)
                {
                    if (player.Role != RoleType.Spectator)
                    {
                        continue;
                    }
                    Global.PlayersVotes.Add(player.Id, 0);
                    if (Global.InWhitelistCommander(player.UserId))
                    {
                        Global.AvailableCommanderPlayerId.Add(player.Id);
                        Global.PlayersVotes = new Dictionary<int, int>();
                        break;
                    }
                }
            }
        }

        internal void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<UseMedicalItemComponent>());
            }
            if (ev.Player.GameObject.GetComponent<MicroHIDDropComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<MicroHIDDropComponent>());
            }
            if (ev.Player.Id == Global.CurrentManagerPlayerId)
            {
                Global.CurrentManagerPlayerId = 0;
            }
            if (ev.Player.Id == Global.CurrentSecurityChiefPlayerId)
            {
                Global.CurrentSecurityChiefPlayerId = 0;
            }
        }

        internal void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (ev.Name.ToLower() == "islegit")
            {
                if (Physics.Raycast((ev.Player.CameraTransform.forward * 1.001f) + ev.Player.GameObject.transform.position, ev.Player.CameraTransform.forward, out RaycastHit hit, 2.0f))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = "Вы не смотрите на проверяемого человека, либо находитесь слишком далеко от него";
                        return;
                    }
                    Player target = Player.Get(hit.transform.GetComponent<QueryProcessor>().PlayerId);
                    if (target == null)
                    {
                        ev.ReturnMessage = "Вы не смотрите на проверяемого человека, либо находитесь слишком далеко от него";
                        return;
                    }
                    if (!AllCards.Contains(target.CurrentItem.id))
                    {
                        ev.ReturnMessage = "У проверяемого нет удостоверения";
                        return;
                    }
                    if (target.Id == Global.CurrentManagerPlayerId && target.CurrentItem.id == ItemType.KeycardFacilityManager)
                    {
                        ev.ReturnMessage = "Перед вами настоящий Директор Зоны";
                        return;
                    }
                    else if (target.Id == Global.CurrentSecurityChiefPlayerId && target.CurrentItem.id == ItemType.KeycardNTFLieutenant)
                    {
                        ev.ReturnMessage = "Перед вами настоящий начальник СБ";
                        return;
                    }
                    else
                    {
                        ev.ReturnMessage = "Перед вами ненастоящий начальник СБ/Директор Зоны";
                        return;
                    }
                }
                ev.ReturnMessage = "Вы не смотрите на проверяемого человека, либо находитесь слишком далеко от него";
                return;
            }
            if (ev.Name.ToLower().Contains("vote"))
            {
                CommanderVoteComponent commanderVoteComponent = GameObject.FindWithTag("FemurBreaker").GetComponent<CommanderVoteComponent>();
                if (commanderVoteComponent == null)
                {
                    ev.ReturnMessage = "Сейчас не проходит голосование";
                    return;
                }

                if (ev.Arguments.Count != 1)
                {
                    ev.ReturnMessage = "Неправильное использование команды!";
                    return;
                }
                if (!commanderVoteComponent.PlayersVotes.ContainsKey(ev.Player.Id))
                {
                    ev.ReturnMessage = "Вы не можете голосовать";
                    return;
                }
                if (commanderVoteComponent.PlayersAlreadyVotes.Contains(ev.Player.Id))
                {
                    ev.ReturnMessage = "Вы уже проголосовали";
                    return;
                }
                if (!int.TryParse(ev.Arguments[0], out int playerId))
                {
                    ev.ReturnMessage = "Это не номер игрока!";
                    return;
                }
                if (!commanderVoteComponent.PlayersVotes.ContainsKey(playerId))
                {
                    ev.ReturnMessage = "Игрок с номером " + playerId + " не найден";
                    return;
                }
                commanderVoteComponent.PlayersVotes[playerId]++;
                commanderVoteComponent.PlayersAlreadyVotes.Add(ev.Player.Id);
                ev.ReturnMessage = "Вы проголосовали за игрока с номером " + playerId;
                return;
            }
            else if (ev.Name.ToLower().Contains("contain") && ev.Name.ToLower().Contains("96"))
            {
                Player scp096 = Player.List.Where(x => x.Role == RoleType.Scp096).FirstOrDefault();
                if (scp096 == default)
                {
                    ev.ReturnMessage = Global._outofscp096;
                    return;
                }
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._outofscp096;
                    return;
                }
                foreach (GameObject gameplayer in PlayerManager.players)
                {
                    if (gameplayer.GetComponent<Contain096OwnerComponent>() != null)
                    {
                        ev.ReturnMessage = Global._alreadycontainproccess096;
                        return;
                    }
                }
                if (Vector3.Distance(ev.Player.Position, scp096.Position) < Global.distanceForContain096And173)
                {

                    ev.Player.GameObject.AddComponent<Contain096OwnerComponent>();
                    ev.Player.GameObject.GetComponent<Contain096OwnerComponent>().owner = ev.Player;
                    ev.Player.GameObject.GetComponent<Contain096OwnerComponent>().scp096 = scp096;
                    ev.ReturnMessage = Global._successstartcontain096 + Global.time_to_contain_096;
                    return;
                }
                else
                {
                    ev.ReturnMessage = Global._outofscp096;
                    return;
                }
            }
            else if (ev.Name.ToLower().Contains("contain") && ev.Name.ToLower().Contains("173"))
            {
                Player scp173 = Player.List.Where(x => x.Role == RoleType.Scp173).FirstOrDefault();
                if (scp173 == default)
                {
                    ev.ReturnMessage = Global._outofscp173;
                    return;
                }
                if (ev.Player.Team == Team.SCP)
                {
                    ev.ReturnMessage = Global._outofscp173;
                    return;
                }
                foreach (GameObject gameplayer in PlayerManager.players)
                {
                    if (gameplayer.GetComponent<Contain173OwnerComponent>() != null)
                    {
                        ev.ReturnMessage = Global._alreadycontainproccess173;
                        return;
                    }
                }
                if (Vector3.Distance(ev.Player.Position, scp173.Position) < Global.distanceForContain096And173)
                {

                    ev.Player.GameObject.AddComponent<Contain173OwnerComponent>();
                    ev.Player.GameObject.GetComponent<Contain173OwnerComponent>().owner = ev.Player;
                    ev.Player.GameObject.GetComponent<Contain173OwnerComponent>().scp173 = scp173;
                    ev.ReturnMessage = Global._successstartcontain173 + Global.time_to_contain_173;
                    return;
                }
                else
                {
                    ev.ReturnMessage = Global._outofscp173;
                    return;
                }
            }
        }

        internal void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Arguments.Count != 2 || ev.Name.ToLower() != "forceclass" || !int.TryParse(ev.Arguments[1], out int roleId) || roleId != 12)
                return;
            if (!int.TryParse(ev.Arguments[0].Substring(0, ev.Arguments[0].Length - 1), out int playerId))
                return;
            if (!Global.AvailableCommanderPlayerId.Contains(playerId))
            {
                Global.AvailableCommanderPlayerId.Add(playerId);
            }
        }

        internal void OnStoppingMedicalItem(StoppingMedicalItemEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<UseMedicalItemComponent>());
            }
        }

        internal void OnSpawning(SpawningEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<FallingBehaviour>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<FallingBehaviour>());
            }
            if (ev.Player.GameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<UseMedicalItemComponent>());
            }
            if (ev.Player.GameObject.GetComponent<MicroHIDDropComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.GameObject.GetComponent<MicroHIDDropComponent>());
            }
            if (ev.Player.Id == Global.CurrentManagerPlayerId)
            {
                Global.CurrentManagerPlayerId = 0;
            }
            if (ev.Player.Id == Global.CurrentSecurityChiefPlayerId)
            {
                Global.CurrentSecurityChiefPlayerId = 0;
            }

            if (ev.RoleType == RoleType.NtfCommander && !Global.InWhitelistCommander(ev.Player.UserId))
            {
                if (Global.AvailableCommanderPlayerId.Contains(ev.Player.Id))
                {
                    Global.AvailableCommanderPlayerId.Remove(ev.Player.Id);
                    return;
                }
                if (Global.AvailableCommanderPlayerId.Count != 0)
                {
                    Player commander = Player.Get(Global.AvailableCommanderPlayerId.First());
                    if (commander != null)
                    {
                        Global.AvailableCommanderPlayerId.Remove(commander.Id);
                        if (commander.Team == Team.MTF)
                        {
                            LateSpawnComponent lateSpawnComponent = ev.Player.GameObject.AddComponent<LateSpawnComponent>();
                            lateSpawnComponent.RoleType = commander.Role;
                            LateSpawnComponent lateSpawnComponent3 = commander.GameObject.AddComponent<LateSpawnComponent>();
                            lateSpawnComponent3.RoleType = RoleType.NtfCommander;
                        }
                        else
                        {
                            LateSpawnComponent lateSpawnComponent = ev.Player.GameObject.AddComponent<LateSpawnComponent>();
                            lateSpawnComponent.RoleType = RoleType.NtfCadet;
                            LateSpawnComponent lateSpawnComponent4 = commander.GameObject.AddComponent<LateSpawnComponent>();
                            lateSpawnComponent4.RoleType = RoleType.NtfCommander;
                        }
                        return;
                    }
                }
                List<Player> spectators = new List<Player>();
                foreach (Player player in Player.List)
                {
                    if (!Global.InWhitelistCommander(player.UserId))
                        continue;
                    if (player.Role == RoleType.Spectator)
                    {
                        spectators.Add(player);
                    }
                }
                LateSpawnComponent lateSpawnComponent2 = ev.Player.GameObject.AddComponent<LateSpawnComponent>();
                lateSpawnComponent2.RoleType = RoleType.NtfCadet;
                if (spectators.Count > 0)
                {
                    System.Random random = new System.Random();
                    Player commander = spectators[random.Next(0, spectators.Count)];
                    LateSpawnComponent lateSpawnComponent5 = commander.GameObject.AddComponent<LateSpawnComponent>();
                    lateSpawnComponent5.RoleType = RoleType.NtfCommander;
                }
                else
                {
                    CommanderVoteComponent commanderVoteComponent = GameObject.FindWithTag("FemurBreaker").AddComponent<CommanderVoteComponent>();
                    commanderVoteComponent.PlayersVotes = Global.PlayersVotes;
                    Global.PlayersVotes = new Dictionary<int, int>();
                }
            }

            if (new RoleType[] { RoleType.Scp93953, RoleType.Scp93989, RoleType.Scp0492, RoleType.Scp049, RoleType.Scp173 }.Contains(ev.RoleType))
            {
                ev.Player.GameObject.AddComponent<FallingBehaviour>();
            }
        }

        internal void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (ev.Player.Role == RoleType.Scientist)
            {
                if (ev.NewItem.id == ItemType.GunE11SR)
                {
                    for (int i = 0; i < ev.Player.Inventory.items.Count; i++)
                    {
                        if (ev.Player.Inventory.items[i].id == ItemType.GunE11SR)
                        {
                            ev.Player.DropItem(ev.Player.Inventory.items[i]);
                            break;
                        }
                    }
                }
                else if (ev.NewItem.id == ItemType.GunLogicer)
                {
                    for (int i = 0; i < ev.Player.Inventory.items.Count; i++)
                    {
                        if (ev.Player.Inventory.items[i].id == ItemType.GunLogicer)
                        {
                            ev.Player.DropItem(ev.Player.Inventory.items[i]);
                            break;
                        }
                    }
                }
            }
            if (ev.OldItem.id == ItemType.MicroHID)
            {
                for (int i = 0; i < ev.Player.Inventory.items.Count; i++)
                {
                    if (ev.Player.Inventory.items[i].id == ItemType.MicroHID)
                    {
                        ev.Player.DropItem(ev.Player.Inventory.items[i]);
                        break;
                    }
                }
            }
        }

        internal void OnPickingUpItem(PickingUpItemEventArgs ev)
        {

            if (ev.Pickup.ItemId == ItemType.GunE11SR || ev.Pickup.ItemId == ItemType.GunLogicer)
            {
                if (ev.Player.Inventory.items.Where(x => x.id == ItemType.GunE11SR).FirstOrDefault() != default || ev.Player.Inventory.items.Where(x => x.id == ItemType.GunLogicer).FirstOrDefault() != default)
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    SetItemModsComponent setItemModsComponent = ev.Player.GameObject.AddComponent<SetItemModsComponent>();
                    setItemModsComponent.ItemType = ev.Pickup.ItemId;
                    setItemModsComponent.Player = ev.Player;
                }
            }
            if (ev.Pickup.ItemId == ItemType.MicroHID)
            {
                ev.Player.GameObject.AddComponent<MicroHIDDropComponent>();
            }
        }

        internal void OnInsertingGeneratorTablet(InsertingGeneratorTabletEventArgs ev)
        {
            if (!AccessGeneratorInsert.Contains(ev.Player.Role))
            {
                ev.IsAllowed = false;
            }
        }

        internal void OnWaitingsForPlayers()
        {
            Global.AvailableCommanderPlayerId = new List<int>();
            Global.CurrentManagerPlayerId = 0;
            Global.CurrentSecurityChiefPlayerId = 0;
            Global.PlayersVotes = new Dictionary<int, int>();
            try
            {
                Global.WhitelistCommander = new List<string>();
                Global.WhitelistCommander = File.ReadAllLines(Global.WhitelistCommanderFullFileName, Encoding.UTF8).ToList();
            }
            catch (Exception)
            {
                Log.Info("Error loading " + nameof(Global.WhitelistCommander));
            }
            try
            {
                Global.WhitelistManager = new List<string>();
                Global.WhitelistManager = File.ReadAllLines(Global.WhitelistManagerFullFileName, Encoding.UTF8).ToList();
            }
            catch (Exception)
            {
                Log.Info("Error loading " + nameof(Global.WhitelistManager));
            }
        }

        private readonly List<RoleType> AccessGeneratorInsert = new List<RoleType>()
        {
            RoleType.Scientist,
            RoleType.FacilityGuard,
            RoleType.NtfCadet,
            RoleType.NtfLieutenant,
            RoleType.NtfScientist,
            RoleType.NtfCommander,
            RoleType.ChaosInsurgency
        };
        private readonly List<ItemType> AllCards = new List<ItemType>()
        {
            ItemType.KeycardContainmentEngineer,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardGuard,
            ItemType.KeycardJanitor,
            ItemType.KeycardNTFCommander,
            ItemType.KeycardNTFLieutenant,
            ItemType.KeycardO5,
            ItemType.KeycardScientist,
            ItemType.KeycardScientistMajor,
            ItemType.KeycardSeniorGuard,
            ItemType.KeycardZoneManager
        };
    }
}