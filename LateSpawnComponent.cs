using EXILED.Extensions;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class LateSpawnComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 0.1f;
        private ReferenceHub PlayerHub;
        public RoleType RoleType = RoleType.Spectator;
        public void Start()
        {
            PlayerHub = Player.GetPlayer(gameObject);
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;
                if (RoleType != RoleType.Spectator)
                    PlayerHub.SetRole(RoleType);

                Destroy(this);
            }
        }
    }
}