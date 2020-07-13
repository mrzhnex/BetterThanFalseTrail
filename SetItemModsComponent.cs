using EXILED.Extensions;
using UnityEngine;

namespace BetterThanFalseTrail
{
    public class SetItemModsComponent : MonoBehaviour
    {
        private float TimeIsUp = 0.1f;
        private float Timer = 0.0f;
        public ItemType ItemType;
        public ReferenceHub PlayerHub;

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                for (int i = 0; i < PlayerHub.inventory.items.Count; i++)
                {
                    if (PlayerHub.inventory.items[i].id == ItemType)
                    {
                        Inventory.SyncItemInfo syncItemInfo = PlayerHub.inventory.items[i];
                        PlayerHub.inventory.items.Remove(PlayerHub.inventory.items[i]);
                        PlayerHub.AddItem(syncItemInfo);
                        break;
                    }
                }
                Destroy(this);
            }
        }
    }
}