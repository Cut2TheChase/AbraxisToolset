using HBS.Text;
using HBS.Collections;
using Necro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbraxisToolset;
using UnityEngine;
using HBS.Pooling;
using HBS.DebugConsole;

namespace Patches
{
    [MonoMod.MonoModPatch("global::Necro.Inventory")]
    class patch_Inventory : Inventory
    {
        [MonoMod.MonoModIgnore]
        private extern void NetworkAddIngredient(string id, int value);

        [MonoMod.MonoModIgnore]
        private extern void NetworkRemoveEquipment(Equippable equip);

        [MonoMod.MonoModIgnore]
        private extern bool RemoveItemFromSlot(Item item, int slot, bool use);

        [MonoMod.MonoModPublic]
        private Dictionary<string, List<Equippable>> equippedObjects;

        public void NetworkAddIngredientProxy(string id, int value)
        {
            NetworkAddIngredient(id, value);
        }

        public void removeEquipped(Equippable item)
        {
            Debug.Log("Hey Lets remove this thing! " + item.ItemDef.id);
            Equippable equippedFromDef = item;
            ItemDef itemDef = item.ItemDef;
            Debug.Log("Hey the itemDef is " + itemDef.id);

            
            Debug.Log("Checking List for j_r_weapon");
            if(this.equippedObjects["j_r_weapon"].Count() > 0)
               for(int j = 0; j < this.equippedObjects["j_r_weapon"].Count(); j++)
                {
                     Debug.Log(this.equippedObjects["j_r_weapon"][j].ItemDef.id);
                }

            //Go through KeyPairs and delete the item from the currently selected inventory
            foreach (KeyValuePair<string, List<Equippable>> keyValuePair in this.equippedObjects)
            {
                for (int i = 0; i < keyValuePair.Value.Count; i++)
                {
                    Equippable equippable = keyValuePair.Value[i];
                    if (equippable != null && equippable.ItemDef.id == item.ItemDef.id)
                    {
                        Debug.Log("Hey we found the item in the inventory~!");
                        this.SetEquippedItem(equippable, false, false, true);
                        if (equippable.ItemDef.kind != ItemDef.Kind.Armor)
                        {
                            equippable.transform.SetParent(null);
                            equippable.OnUnequip();
                        }
                        List<Equippable> list = null;
                        if (this.equippedObjects.TryGetValue(equippable.ItemDef.boneRef, out list))
                        {
                            list.Remove(equippable);
                        }
                        this.NetworkRemoveEquipment(equippable);
                        equippable.Despawn<Equippable>();
                    }
                }
            }

        }
    }
}
