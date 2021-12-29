using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    public Dictionary<EquipableItem, EquipmentSlot> EquipmentItems = new Dictionary<EquipableItem, EquipmentSlot>();
    public List<EquipmentSlot> EquipmentSlots;

    private void Update()
    {
        base.Update();
    }

    private void Start()
    {
        base.Start();
        //Equipment slots events
        InventoryEvents.current.onPointerEnterEquipmentSlot += this.EquipmentSlot_SlotEntered;
        InventoryEvents.current.onPointerExitEquipmentSlot += this.EquipmentSlot_SlotExited;
        InventoryEvents.current.onPointerClickEquipmentSlot += this.EquipmentSlot_SlotClicked;

        InventoryEvents.current.onItemEquipped += this.ItemEquipped;
        InventoryEvents.current.onItemUnEquipped += this.ItemUnEquipped;
    }

    private void ItemUnEquipped(EquipmentSlot slot, EquipableItem item)
    {
        EquipmentItems.Remove(item);
    }

    private void ItemEquipped(EquipmentSlot slot, EquipableItem item)
    {
        EquipmentItems.Add(item, slot);
    }

    private void EquipmentSlot_SlotExited(EquipmentSlot slot)
    {
        if (slot.isOccupied)
        {
            InventoryEvents.current.PointerExitItem(slot.Item);
            this.HighlightEquipmentSlot(slot, Colors.Blue);
        }
        else
        {
            this.HighlightEquipmentSlot(slot, Colors.Clear);
        }
    }

    private void EquipmentSlot_SlotClicked(EquipmentSlot slot)
    {
        if (!isItemSelected)
        {
            if (slot.isOccupied)
            {
                InventoryEvents.current.PointerExitItem(slot.Item);
                //pickup the item
                isItemSelected = true;
                selectedItem = slot.Item;
                selectedFrom = this;
                this.HighlightEquipmentSlot(slot, Colors.Clear);
                InventoryEvents.current.UnEquipItem(slot, (EquipableItem)slot.Item);
                slot.Item = null;
                slot.isOccupied = false;
                this.SetSlotRaycastTartget(selectedItem.itemSize.x % 2 == 0, selectedItem.itemSize.y % 2 == 0);
            }
        }
        else
        {
            if (selectedItem.GetType().IsSubclassOf(typeof(EquipableItem)) || selectedItem.GetType() == typeof(EquipableItem))
            {
                //swap with selected Item
                if (slot.isOccupied)
                {
                    if (this.CanEquipItem((EquipableItem)selectedItem, slot))
                    {
                        var tempItem = slot.Item;
                        //unequip
                        InventoryEvents.current.UnEquipItem(slot, (EquipableItem)slot.Item);

                        //equip
                        slot.Item = selectedItem;
                        InventoryEvents.current.EquipItem(slot, (EquipableItem)slot.Item);

                        selectedItem = tempItem;
                        selectedFrom = this;
                        this.HighlightEquipmentSlot(slot, Colors.Blue);
                        this.SetSlotRaycastTartget(selectedItem.itemSize.x % 2 == 0, selectedItem.itemSize.y % 2 == 0);
                    }
                }
                //drop the selected item in the slot if the type matches
                else
                {
                    if (this.CanEquipItem((EquipableItem)selectedItem, slot))
                    {
                        slot.Item = selectedItem;
                        slot.isOccupied = true;
                        InventoryEvents.current.EquipItem(slot, (EquipableItem)selectedItem);
                        this.HighlightEquipmentSlot(slot, Colors.Green);
                        this.SetSlotRaycastTartget(false, false);
                        isItemSelected = false;
                        selectedItem = null;
                        selectedFrom = null;
                        InventoryEvents.current.PointerEnterItem(slot.Item);
                    }
                }
            }
        }
    }

    private void HighlightEquipmentSlot(EquipmentSlot slot, Color color)
    {
        slot.GetComponent<Image>().color = color;
    }

    private void EquipmentSlot_SlotEntered(EquipmentSlot slot)
    {

        if (!isItemSelected)
        {
            if (slot.isOccupied)
            {
                InventoryEvents.current.PointerEnterItem(slot.Item);
                this.HighlightEquipmentSlot(slot, Colors.Green);
            }
        }
        else
        {
            if (selectedItem.GetType().IsSubclassOf(typeof(EquipableItem)) || selectedItem.GetType() == typeof(EquipableItem))
            {
                if (this.CanEquipItem((EquipableItem)selectedItem, slot))
                {
                    this.HighlightEquipmentSlot(slot, Colors.Green);
                }
                else
                {
                    this.HighlightEquipmentSlot(slot, Colors.Red);
                }
            }
            else
            {
                this.HighlightEquipmentSlot(slot, Colors.Red);
            }
        }
    }

    private bool CanEquipItem(EquipableItem selectedItem, EquipmentSlot slot)
    {
        if (selectedItem.itemType == slot.SlotType)
        {
            if (slot.SlotType == EquipmentSlotType.MainHand)
            {
                Weapon weapon = (Weapon)selectedItem;
                if (weapon != null)
                {
                    if (weapon.weaponType == WeaponType.TwoHandedSword || weapon.weaponType == WeaponType.TwoHandedAxe)
                    {
                        if (EquipmentSlots.Find(x => x.SlotType == EquipmentSlotType.OffHand).isOccupied)
                        {
                            return false;
                        }
                    }
                    else if (weapon.weaponType == WeaponType.OneHandedSword || weapon.weaponType == WeaponType.OneHandedAxe || weapon.weaponType == WeaponType.Dagger)
                    {
                        if(EquipmentSlots.Find(x => x.SlotType == EquipmentSlotType.OffHand).isOccupied)
                        {
                            if (EquipmentSlots.Find(x => x.SlotType == EquipmentSlotType.OffHand).Item.GetType() != typeof(Shield))
                            {                       
                                return false;                        
                            }
                        }                        
                    }
                    else if (weapon.weaponType == WeaponType.Bow)
                    {
                        if(EquipmentSlots.Find(x => x.SlotType == EquipmentSlotType.OffHand).isOccupied)
                        {
                            if (EquipmentSlots.Find(x => x.SlotType == EquipmentSlotType.OffHand).Item.GetType() != typeof(Quiver))
                            {                       
                                return false;                        
                            }
                        }                        
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (slot.SlotType == EquipmentSlotType.OffHand)
            {
                Weapon weapon = (Weapon)EquipmentSlots.Find(x => x.SlotType == EquipmentSlotType.MainHand).Item;
                if (selectedItem.GetType() == typeof(Shield))
                {                    
                    if (weapon != null && (weapon.weaponType == WeaponType.TwoHandedSword || weapon.weaponType == WeaponType.TwoHandedAxe || weapon.weaponType == WeaponType.Bow))
                    {
                        return false;
                    }
                }
                else if (selectedItem.GetType() == typeof(Quiver))
                {                   
                    if (weapon != null && (weapon.weaponType != WeaponType.Bow))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        return false;
    }

}