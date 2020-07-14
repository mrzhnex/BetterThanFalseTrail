using EXILED.Extensions;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class MicroHIDDropComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private ReferenceHub PlayerHub;

        public void Start()
        {
            PlayerHub = Player.GetPlayer(gameObject);
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > Global.TimeToPickupMicroHID)
            {
                MicroHIDDrop();
                Destroy(this);
            }
        }
        private void MicroHIDDrop()
        {
            for (int i = 0; i < PlayerHub.inventory.items.Count; i++)
            {
                if (PlayerHub.inventory.items[i].id == ItemType.MicroHID)
                {
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, "Вы обронили Micro H.I.D.", true);
                    Map.SpawnItem(ItemType.MicroHID, PlayerHub.inventory.items[i].durability, PlayerHub.gameObject.transform.position, PlayerHub.gameObject.transform.rotation, PlayerHub.inventory.items[i].modSight, PlayerHub.inventory.items[i].modBarrel, PlayerHub.inventory.items[i].modOther);
                    PlayerHub.inventory.items.Remove(PlayerHub.inventory.items[i]);
                    break;
                }
            }
        }
    }
}