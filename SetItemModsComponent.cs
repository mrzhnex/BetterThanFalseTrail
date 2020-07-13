using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace BetterThanFalseTrail
{
    public class SetItemModsComponent : MonoBehaviour
    {
        private readonly float TimeIsUp = 0.1f;
        private float Timer = 0.0f;
        public ItemType ItemType;
        public ReferenceHub PlayerHub;
        private bool Remove = false;
        private Inventory.SyncItemInfo SyncItemInfo;
        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                if (Remove)
                {
                    PlayerHub.AddItem(SyncItemInfo);
                    Destroy(this);
                }
                else
                {
                    for (int i = 0; i < PlayerHub.inventory.items.Count; i++)
                    {
                        if (PlayerHub.inventory.items[i].id == ItemType)
                        {
                            SyncItemInfo = PlayerHub.inventory.items[i];
                            PlayerHub.inventory.items.Remove(PlayerHub.inventory.items[i]);
                            break;
                        }
                    }
                    Remove = true;
                }
            }
        }
    }
}