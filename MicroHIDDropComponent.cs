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
                if (PlayerHub.GetCurrentItem().id != ItemType.MicroHID || PlayerHub.inventory.curItem == ItemType.None)
                {
                    for (int i = 0; i < PlayerHub.inventory.items.Count; i++)
                    {
                        if (PlayerHub.inventory.items[i].id == ItemType.MicroHID)
                        {
                            Map.SpawnItem(ItemType.MicroHID, PlayerHub.inventory.items[i].durability, PlayerHub.gameObject.transform.position, PlayerHub.gameObject.transform.rotation, PlayerHub.inventory.items[i].modSight, PlayerHub.inventory.items[i].modBarrel, PlayerHub.inventory.items[i].modOther);
                            PlayerHub.inventory.items.Remove(PlayerHub.inventory.items[i]);
                            break;
                        }
                    }
                }
                Destroy(this);
            }
        }
    }
}