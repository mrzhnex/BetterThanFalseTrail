using Exiled.API.Features;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class MicroHIDDropComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private Player Player;

        public void Start()
        {
            Player = Player.Get(gameObject);
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > Global.TimeToPickupMicroHID)
            {
                if (Player.CurrentItem.id != ItemType.MicroHID)
                {
                    MicroHIDDrop();
                }
                Destroy(this);
            }
        }
        private void MicroHIDDrop()
        {
            for (int i = 0; i < Player.Inventory.items.Count; i++)
            {
                if (Player.Inventory.items[i].id == ItemType.MicroHID)
                {
                    Player.ClearBroadcasts();
                    Player.Broadcast(10, "Вы обронили Micro H.I.D.", Broadcast.BroadcastFlags.Normal);
                    Player.DropItem(Player.Inventory.items[i]);
                    break;
                }
            }
        }
    }
}