using Exiled.API.Features;
using UnityEngine;

namespace BetterThanFalseTrail
{
    public class SetItemModsComponent : MonoBehaviour
    {
        private readonly float TimeIsUp = 0.1f;
        private float Timer = 0.0f;
        public ItemType ItemType;
        public Player Player;
        private bool Remove = false;
        private Inventory.SyncItemInfo SyncItemInfo;
        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                if (Remove)
                {
                    Player.AddItem(SyncItemInfo);
                    Destroy(this);
                }
                else
                {
                    for (int i = 0; i < Player.Inventory.items.Count; i++)
                    {
                        if (Player.Inventory.items[i].id == ItemType)
                        {
                            SyncItemInfo = Player.Inventory.items[i];
                            Player.Inventory.items.Remove(Player.Inventory.items[i]);
                            break;
                        }
                    }
                    Remove = true;
                }
            }
        }
    }
}