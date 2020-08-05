using Exiled.API.Features;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class UseMedicalItemComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 0.2f;
        private Vector3 PreviosPosition;
        private Player Player;
        public ItemType ItemType;

        public void Start()
        {
            Player = Player.Get(gameObject);
            PreviosPosition = transform.position;
        }

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;

                if (Vector3.Distance(transform.position, PreviosPosition) >= 1.4f)
                {
                    CancelUseMedicalItem();
                    Destroy(this);
                }
                PreviosPosition = transform.position;
            }
        }

        private void CancelUseMedicalItem()
        {
            Player.GameObject.GetComponent<ConsumableAndWearableItems>().SendRpc(ConsumableAndWearableItems.HealAnimation.CancelHealing, 0);
            for (int i = 0; i < Player.Inventory.items.Count; i++)
            {
                if (Player.Inventory.items[i].id == ItemType && Player.Inventory.items[i].uniq == Player.CurrentItem.uniq)
                {
                    foreach (ConsumableAndWearableItems.UsableItem usableItem in Player.GameObject.GetComponent<ConsumableAndWearableItems>().usableItems)
                    {
                        if (usableItem.inventoryID == ItemType && usableItem.cancelableTime > 0f)
                        {
                            Player.GameObject.GetComponent<ConsumableAndWearableItems>()._cancel = true;
                        }
                    }
                    Player.ClearBroadcasts();
                    Player.Broadcast(10, "Вы обронили медикаменты, так как пытались использовать их на бегу", Broadcast.BroadcastFlags.Normal);
                    Player.DropItem(Player.Inventory.items[i]);
                    break;
                }
            }
        }
    }
}