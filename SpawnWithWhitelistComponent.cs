using Exiled.API.Features;
using System.Collections.Generic;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class SpawnWithWhitelistComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 1.0f;
        private readonly System.Random rand = new System.Random();

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;

                List<Player> scientists = new List<Player>();
                List<Player> guards = new List<Player>();

                foreach (Player player in Player.List)
                {
                    if (player.Role == RoleType.Scp049 && !Global.InWhitelistCommander(player.UserId))
                    {
                        player.SetRole(RoleType.Scp0492, true);
                    }
                    if (!Global.InWhitelistManager(player.UserId))
                        continue;
                    if (player.Role == RoleType.Scientist)
                    {
                        scientists.Add(player);
                    }
                    else if (player.Role == RoleType.FacilityGuard)
                    {
                        guards.Add(player);
                    }
                }

                if (scientists.Count > 0)
                {
                    Player manager = scientists[rand.Next(0, scientists.Count)];
                    manager.AddItem(ItemType.KeycardFacilityManager);
                    manager.ClearBroadcasts();
                    manager.Broadcast(30, "<color=#876c99>Вы - директор комплекса</color>", Broadcast.BroadcastFlags.Normal);
                    Global.CurrentManagerPlayerId = manager.Id;
                }
                if (guards.Count > 0)
                {
                    Player chiefguard = guards[rand.Next(0, guards.Count)];
                    chiefguard.AddItem(ItemType.KeycardNTFLieutenant);
                    chiefguard.ClearBroadcasts();
                    chiefguard.Broadcast(30, "<color=#876c99>Вы - сержант службы безопасности</color>", Broadcast.BroadcastFlags.Normal);
                    Global.CurrentSecurityChiefPlayerId = chiefguard.Id;
                }

                Destroy(this);
            }
        }
    }
}