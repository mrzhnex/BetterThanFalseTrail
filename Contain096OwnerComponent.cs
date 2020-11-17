using Exiled.API.Features;
using UnityEngine;

namespace BetterThanFalseTrail
{
    public class Contain096OwnerComponent : MonoBehaviour
    {
        public Player owner;
        public Player scp096;
        private float timer = 0.0f;
        private readonly float time_is_up = 1.0f;
        private float progress = Global.time_to_contain_096;

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= time_is_up)
            {
                timer = 0f;
                progress -= time_is_up;
                owner.ClearBroadcasts();
                owner.Broadcast(1, Global._successstartcontain096 + progress, Broadcast.BroadcastFlags.Normal);
                scp096.ClearBroadcasts();
                scp096.Broadcast(1, Global._noticescp096, Broadcast.BroadcastFlags.Normal);
                if (progress <= 0.0f)
                {
                    Contain();
                }
                if (Vector3.Distance(transform.position, scp096.Position) > Global.distanceForContain096And173)
                {
                    owner.ClearBroadcasts();
                    owner.Broadcast(10, Global._failedcontain096and173, Broadcast.BroadcastFlags.Normal);
                    Destroy(gameObject.GetComponent<Contain096OwnerComponent>());
                }
            }
        }

        private void Contain()
        {
            scp096.SetRole(RoleType.Spectator);
            Destroy(gameObject.GetComponent<Contain096OwnerComponent>());
        }
    }
}