using Exiled.API.Features;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class FallingBehaviour : MonoBehaviour
    {
        private Player Player { get; set; }
        private float Timer { get; set; } = 0.0f;
        private float TimeIsUp { get; set; } = 0.1f;
        private float SaveTime { get; set; } = 3.0f;
        private float PreviousHeight { get; set; } = 0.0f;

        public void Start()
        {
            Player = Player.Get(gameObject);
            PreviousHeight = Player.Position.y;
        }

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;
                if (SaveTime < 0.0f)
                {
                    if (PreviousHeight > Player.Position.y + 2.5f && PreviousHeight <= Player.Position.y + 30.0f && !Player.IsJumping)
                        Player.Hurt((PreviousHeight - Player.Position.y) * 110.0f, Player, DamageTypes.Falldown);
                }
                else
                    SaveTime -= TimeIsUp;
                if (!Player.IsJumping)
                    PreviousHeight = Player.Position.y;
            }
        }
    }
}