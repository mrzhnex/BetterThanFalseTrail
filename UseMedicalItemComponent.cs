using EXILED.Extensions;
using UnityEngine;

namespace BetterThanFalseTrail
{
    internal class UseMedicalItemComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 0.2f;
        private Vector3 PreviosPosition;
        private ReferenceHub PlayerHub;
        public ItemType ItemType;

        public void Start()
        {
            PlayerHub = Player.GetPlayer(gameObject);
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
            PlayerHub.gameObject.GetComponent<ConsumableAndWearableItems>().SendRpc(ConsumableAndWearableItems.HealAnimation.CancelHealing, 0);
            for (int i = 0; i < PlayerHub.inventory.items.Count; i++)
            {
                if (PlayerHub.inventory.items[i].id == ItemType && PlayerHub.inventory.items[i].uniq == PlayerHub.GetCurrentItem().uniq)
                {
                    foreach (ConsumableAndWearableItems.UsableItem usableItem in PlayerHub.gameObject.GetComponent<ConsumableAndWearableItems>().usableItems)
                    {
                        if (usableItem.inventoryID == ItemType && usableItem.cancelableTime > 0f)
                        {
                            PlayerHub.gameObject.GetComponent<ConsumableAndWearableItems>().cancel = true;
                        }
                    }
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, "Вы обронили медикаменты, так как вы пытались использовать их на бегу", true);
                    Map.SpawnItem(ItemType, PlayerHub.inventory.items[i].durability, PlayerHub.gameObject.transform.position, PlayerHub.gameObject.transform.rotation, PlayerHub.inventory.items[i].modSight, PlayerHub.inventory.items[i].modBarrel, PlayerHub.inventory.items[i].modOther);
                    PlayerHub.inventory.items.Remove(PlayerHub.inventory.items[i]);
                    break;
                }
            }
        }
    }
}