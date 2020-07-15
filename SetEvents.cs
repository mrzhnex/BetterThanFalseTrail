using EXILED;
using EXILED.Extensions;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class SetEvents
    {
        internal void OnItemChanged(ItemChangedEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.Scientist)
            {
                if (ev.NewItem.id == ItemType.GunE11SR)
                {
                    for (int i = 0; i < ev.Player.inventory.items.Count; i++)
                    {
                        if (ev.Player.inventory.items[i].id == ItemType.GunE11SR)
                        {
                            Map.SpawnItem(ItemType.GunE11SR, ev.Player.inventory.items[i].durability, ev.Player.gameObject.transform.position, ev.Player.gameObject.transform.rotation, ev.Player.inventory.items[i].modSight, ev.Player.inventory.items[i].modBarrel, ev.Player.inventory.items[i].modOther);
                            ev.Player.inventory.items.Remove(ev.Player.inventory.items[i]);
                            break;
                        }
                    }
                }
                else if (ev.NewItem.id == ItemType.GunLogicer)
                {
                    for (int i = 0; i < ev.Player.inventory.items.Count; i++)
                    {
                        if (ev.Player.inventory.items[i].id == ItemType.GunLogicer)
                        {
                            Map.SpawnItem(ItemType.GunLogicer, ev.Player.inventory.items[i].durability, ev.Player.gameObject.transform.position, ev.Player.gameObject.transform.rotation, ev.Player.inventory.items[i].modSight, ev.Player.inventory.items[i].modBarrel, ev.Player.inventory.items[i].modOther);
                            ev.Player.inventory.items.Remove(ev.Player.inventory.items[i]);
                            break;
                        }
                    }
                }
            }
            if (ev.OldItem.id == ItemType.MicroHID)
            {
                for (int i = 0; i < ev.Player.inventory.items.Count; i++)
                {
                    if (ev.Player.inventory.items[i].id == ItemType.MicroHID)
                    {
                        Map.SpawnItem(ItemType.MicroHID, ev.Player.inventory.items[i].durability, ev.Player.gameObject.transform.position, ev.Player.gameObject.transform.rotation, ev.Player.inventory.items[i].modSight, ev.Player.inventory.items[i].modBarrel, ev.Player.inventory.items[i].modOther);
                        ev.Player.inventory.items.Remove(ev.Player.inventory.items[i]);
                        break;
                    }
                }
            }
        }

        internal void OnTeamRespawn(ref TeamRespawnEvent ev)
        {
            if (!ev.IsChaos)
            {
                foreach (ReferenceHub referenceHub in ev.ToRespawn)
                {
                    Global.PlayersVotes.Add(referenceHub.GetPlayerId(), 0);
                    if (Global.InWhitelistCommander(referenceHub.GetUserId()))
                    {
                        Global.AvailableCommanderPlayerId = referenceHub.GetPlayerId();
                        Global.PlayersVotes = new Dictionary<int, int>();
                        break;
                    }
                }
            }
        }

        internal void OnConsoleCommand(ConsoleCommandEvent ev)
        {
            if (ev.Command.ToLower() == "islegit")
            {
                if (Physics.Raycast((ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward * 1.001f) + ev.Player.gameObject.transform.position, ev.Player.gameObject.GetComponent<Scp049PlayerScript>().plyCam.transform.forward, out RaycastHit hit, 2.0f))
                {
                    if (hit.transform.GetComponent<QueryProcessor>() == null)
                    {
                        ev.ReturnMessage = "Вы не смотрите на проверяемого человека, либо находитесь слишком далеко от него";
                        return;
                    }
                    ReferenceHub target = Player.GetPlayer(hit.transform.GetComponent<QueryProcessor>().PlayerId);
                    if (target == null)
                    {
                        ev.ReturnMessage = "Вы не смотрите на проверяемого человека, либо находитесь слишком далеко от него";
                        return;
                    }
                    if (!AllCards.Contains(target.GetCurrentItem().id))
                    {
                        ev.ReturnMessage = "У проверяемого нет удостоверения";
                        return;
                    }
                    if (target.GetPlayerId() == Global.CurrentManagerPlayerId && target.GetCurrentItem().id == ItemType.KeycardFacilityManager)
                    {
                        ev.ReturnMessage = "Перед вами настоящий Директор Зоны";
                        return;
                    }
                    else if (target.GetPlayerId() == Global.CurrentSecurityChiefPlayerId && target.GetCurrentItem().id == ItemType.KeycardNTFLieutenant)
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
            if (ev.Command.ToLower().Contains("vote"))
            {
                CommanderVoteComponent commanderVoteComponent = GameObject.FindWithTag("FemurBreaker").GetComponent<CommanderVoteComponent>();
                if (commanderVoteComponent == null)
                {
                    ev.ReturnMessage = "Сейчас не проходит голосование";
                    return;
                }

                string[] args = ev.Command.Split(' ');
                if (args.Length != 2)
                {
                    ev.ReturnMessage = "Неправильное использование команды!";
                    return;
                }
                if (!commanderVoteComponent.PlayersVotes.ContainsKey(ev.Player.GetPlayerId()))
                {
                    ev.ReturnMessage = "Вы не можете голосовать";
                    return;
                }
                if (commanderVoteComponent.PlayersAlreadyVotes.Contains(ev.Player.GetPlayerId()))
                {
                    ev.ReturnMessage = "Вы уже проголосовали";
                    return;
                }
                if (!int.TryParse(args[1], out int playerId))
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
                commanderVoteComponent.PlayersAlreadyVotes.Add(ev.Player.GetPlayerId());
                ev.ReturnMessage = "Вы проголосовали за игрока с номером " + playerId;
                return;
            }
        }

        internal void OnRoundStart()
        {
            GameObject.FindWithTag("FemurBreaker").AddComponent<SpawnWithWhitelistComponent>();
        }

        internal void OnWaitingsForPlayers()
        {
            Global.AvailableCommanderPlayerId = 0;
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

        internal void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<UseMedicalItemComponent>());
            }
            if (ev.Player.gameObject.GetComponent<MicroHIDDropComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<MicroHIDDropComponent>());
            }
            if (ev.Player.GetPlayerId() == Global.CurrentManagerPlayerId)
            {
                Global.CurrentManagerPlayerId = 0;
            }
            if (ev.Player.GetPlayerId() == Global.CurrentSecurityChiefPlayerId)
            {
                Global.CurrentSecurityChiefPlayerId = 0;
            }

            if (ev.Role == RoleType.NtfCommander && !Global.InWhitelistCommander(ev.Player.GetUserId()))
            {
                if (Global.AvailableCommanderPlayerId == ev.Player.GetPlayerId())
                {
                    Global.AvailableCommanderPlayerId = 0;
                    return;
                }
                if (Global.AvailableCommanderPlayerId != 0)
                {
                    ReferenceHub commander = Player.GetPlayer(Global.AvailableCommanderPlayerId);
                    if (commander != null)
                    {
                        if (commander.GetTeam() == Team.MTF)
                        {
                            LateSpawnComponent lateSpawnComponent = ev.Player.gameObject.AddComponent<LateSpawnComponent>();
                            lateSpawnComponent.RoleType = commander.GetRole();
                            commander.SetRole(RoleType.NtfCommander);
                        }
                        else
                        {
                            LateSpawnComponent lateSpawnComponent = ev.Player.gameObject.AddComponent<LateSpawnComponent>();
                            lateSpawnComponent.RoleType = RoleType.NtfCadet;
                            commander.SetRole(RoleType.NtfCommander);
                        }
                        return;
                    }
                }
                List<ReferenceHub> spectators = new List<ReferenceHub>();
                foreach (ReferenceHub referenceHub in Player.GetHubs())
                {
                    if (!Global.InWhitelistCommander(referenceHub.GetUserId()))
                        continue;
                    if (referenceHub.GetRole() == RoleType.Spectator)
                    {
                        spectators.Add(referenceHub);
                    }
                }
                LateSpawnComponent lateSpawnComponent2 = ev.Player.gameObject.AddComponent<LateSpawnComponent>();
                lateSpawnComponent2.RoleType = RoleType.NtfCadet;
                if (spectators.Count > 0)
                {
                    System.Random random = new System.Random();
                    ReferenceHub commander = spectators[random.Next(0, spectators.Count)];
                    commander.SetRole(RoleType.NtfCommander);
                }
                else
                {
                    CommanderVoteComponent commanderVoteComponent = GameObject.FindWithTag("FemurBreaker").AddComponent<CommanderVoteComponent>();
                    commanderVoteComponent.PlayersVotes = Global.PlayersVotes;
                    Global.PlayersVotes = new Dictionary<int, int>();
                }
            }
        }

        internal void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<UseMedicalItemComponent>());
            }
            if (ev.Player.gameObject.GetComponent<MicroHIDDropComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<MicroHIDDropComponent>());
            }
            if (ev.Player.GetPlayerId() == Global.CurrentManagerPlayerId)
            {
                Global.CurrentManagerPlayerId = 0;
            }
            if (ev.Player.GetPlayerId() == Global.CurrentSecurityChiefPlayerId)
            {
                Global.CurrentSecurityChiefPlayerId = 0;
            }
        }

        internal void OnCancelMedicalItem(MedicalItemEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<UseMedicalItemComponent>());
            }
        }

        internal void OnUsedMedicalItem(UsedMedicalItemEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<UseMedicalItemComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<UseMedicalItemComponent>());
            }
        }

        internal void OnUseMedicalItem(MedicalItemEvent ev)
        {
            if (ev.Item != ItemType.SCP500)
            {
                if (ev.Player.gameObject.GetComponent<UseMedicalItemComponent>())
                {
                    UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<UseMedicalItemComponent>());
                }
                UseMedicalItemComponent useMedicalItemComponent = ev.Player.gameObject.AddComponent<UseMedicalItemComponent>();
                useMedicalItemComponent.ItemType = ev.Item;
            }
        }

        internal void OnGeneratorInserted(ref GeneratorInsertTabletEvent ev)
        {
            if (!AccessGeneratorInsert.Contains(ev.Player.GetRole()))
            {
                ev.Allow = false;
            }
        }

        internal void OnPickupItem(ref PickupItemEvent ev)
        {
            if (ev.Item.info.itemId == ItemType.GunE11SR || ev.Item.info.itemId == ItemType.GunLogicer)
            {
                if (ev.Player.inventory.items.Where(x => x.id == ItemType.GunE11SR).FirstOrDefault() != default || ev.Player.inventory.items.Where(x => x.id == ItemType.GunLogicer).FirstOrDefault() != default)
                {
                    ev.Allow = false;
                }
                else
                {
                    SetItemModsComponent setItemModsComponent = ev.Player.gameObject.AddComponent<SetItemModsComponent>();
                    setItemModsComponent.ItemType = ev.Item.info.itemId;
                    setItemModsComponent.PlayerHub = ev.Player;
                }
            }
            if (ev.Item.info.itemId == ItemType.MicroHID)
            {
                ev.Player.gameObject.AddComponent<MicroHIDDropComponent>();
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