using Exiled.API.Features;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class LateSpawnComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 0.1f;
        private Player Player;
        public RoleType RoleType = RoleType.Spectator;
        public void Start()
        {
            Player = Player.Get(gameObject);
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;
                if (RoleType != RoleType.Spectator)
                    Player.SetRole(RoleType);

                Destroy(this);
            }
        }
    }
}