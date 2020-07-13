﻿using EXILED;
using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace BetterThanFalseTrail
{
    internal class SetEvents
    {
        internal void OnItemChanged(ItemChangedEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.Scientist)
            {
                if (ev.NewItem.id == ItemType.GunE11SR)
                {
                    for (int i = 0; i < ev.Player.inventory.items.Count; i++)
                    {
                        if (ev.Player.inventory.items[i].id == ItemType.GunE11SR)
                        {
                            Map.SpawnItem(ItemType.GunE11SR, ev.Player.inventory.items[i].durability, ev.Player.gameObject.transform.position, ev.Player.gameObject.transform.rotation, ev.Player.inventory.items[i].modSight, ev.Player.inventory.items[i].modBarrel, ev.Player.inventory.items[i].modOther);
                            ev.Player.inventory.items.Remove(ev.Player.inventory.items[i]);
                            break;
                        }
                    }
                }
                else if (ev.NewItem.id == ItemType.GunLogicer)
                {
                    for (int i = 0; i < ev.Player.inventory.items.Count; i++)
                    {
                        if (ev.Player.inventory.items[i].id == ItemType.GunLogicer)
                        {
                            Map.SpawnItem(ItemType.GunLogicer, ev.Player.inventory.items[i].durability, ev.Player.gameObject.transform.position, ev.Player.gameObject.transform.rotation, ev.Player.inventory.items[i].modSight, ev.Player.inventory.items[i].modBarrel, ev.Player.inventory.items[i].modOther);
                            ev.Player.inventory.items.Remove(ev.Player.inventory.items[i]);
                            break;
                        }
                    }
                }
            }
        }

        internal void OnGeneratorInserted(ref GeneratorInsertTabletEvent ev)
        {
            if (!AccessGeneratorInsert.Contains(ev.Player.GetRole()))
            {
                ev.Allow = false;
            }
        }

        internal void OnPickupItem(ref PickupItemEvent ev)
        {
            if (ev.Item.info.itemId == ItemType.GunE11SR || ev.Item.info.itemId == ItemType.GunLogicer)
            {
                if (ev.Player.inventory.items.Where(x => x.id == ItemType.GunE11SR).FirstOrDefault() != default || ev.Player.inventory.items.Where(x => x.id == ItemType.GunLogicer).FirstOrDefault() != default)
                {
                    ev.Allow = false;
                }
                else
                {
                    SetItemModsComponent setItemModsComponent = ev.Player.gameObject.AddComponent<SetItemModsComponent>();
                    setItemModsComponent.ItemType = ev.Item.info.itemId;
                    setItemModsComponent.PlayerHub = ev.Player;
                }
            }
        }

        private readonly List<RoleType> AccessGeneratorInsert = new List<RoleType>()
        {
            RoleType.Scientist,
            RoleType.FacilityGuard,
            RoleType.NtfCadet,
            RoleType.NtfLieutenant,
            RoleType.NtfScientist,
            RoleType.NtfCommander,
            RoleType.ChaosInsurgency
        };
    }
}