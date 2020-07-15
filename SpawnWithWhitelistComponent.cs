using EXILED.Extensions;
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

                List<ReferenceHub> scientists = new List<ReferenceHub>();
                List<ReferenceHub> guards = new List<ReferenceHub>();

                foreach (ReferenceHub referenceHub in Player.GetHubs())
                {
                    if (referenceHub.GetRole() == RoleType.Scp049 && !Global.InWhitelistCommander(referenceHub.GetUserId()))
                    {
                        referenceHub.SetRole(RoleType.Scp0492, true);
                    }
                    if (!Global.InWhitelistManager(referenceHub.GetUserId()))
                        continue;
                    if (referenceHub.GetRole() == RoleType.Scientist)
                    {
                        scientists.Add(referenceHub);
                    }
                    else if (referenceHub.GetRole() == RoleType.FacilityGuard)
                    {
                        guards.Add(referenceHub);
                    }
                }

                if (scientists.Count > 0)
                {
                    ReferenceHub manager = scientists[rand.Next(0, scientists.Count)];
                    manager.AddItem(ItemType.KeycardFacilityManager);
                    manager.ClearBroadcasts();
                    manager.Broadcast(30, "<color=#876c99>Вы - директор комплекса</color>", true);
                    Global.CurrentManagerPlayerId = manager.GetPlayerId();
                }
                if (guards.Count > 0)
                {
                    ReferenceHub chiefguard = guards[rand.Next(0, guards.Count)];
                    chiefguard.AddItem(ItemType.KeycardNTFLieutenant);
                    chiefguard.ClearBroadcasts();
                    chiefguard.Broadcast(30, "<color=#876c99>Вы - начальник службы безопасности</color>", true);
                    Global.CurrentSecurityChiefPlayerId = chiefguard.GetPlayerId();
                }

                Destroy(this);
            }
        }
    }
}