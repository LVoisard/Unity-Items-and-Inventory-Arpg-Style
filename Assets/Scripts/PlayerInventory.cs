using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{

    public IDictionary<BaseItem, EquipmentSlotType> EquipmentItems { get; set; }    

    private void Start()
    {
        base.Start();
        //Equipment slots events
        InventoryEvents.current.onPointerEnterEquipmentSlot += this.EquipmentSlot_SlotEntered;
        InventoryEvents.current.onPointerExitEquipmentSlot += this.EquipmentSlot_SlotExited;
        InventoryEvents.current.onPointerClickEquipmentSlot += this.EquipmentSlot_SlotClicked;
    }

    private void EquipmentSlot_SlotExited(EquipmentSlot slot)
    {
        if (slot.isOccupied)
        {
            this.HighlightEquipmentSlot(slot, Colors.Blue);
        }
        else
        {
            this.HighlightEquipmentSlot(slot, Colors.Clear);
        }
    }

    private void EquipmentSlot_SlotClicked(EquipmentSlot slot)
    {
        // if (this != selectedFrom && selectedFrom != null)
        // {
        //     return;
        // }

        print(this.transform.name);
        if (!isItemSelected)
        {
            if (slot.isOccupied)
            {
                //pickup the item
                isItemSelected = true;
                selectedItem = slot.Item;
                selectedFrom = this;
                this.HighlightEquipmentSlot(slot, Colors.Clear);
                InventoryEvents.current.UnEquipItem(slot, slot.Item);
                slot.Item = null;
                slot.isOccupied = false;
                this.SetSlotRaycastTartget(selectedItem.itemSize.x % 2 == 0, selectedItem.itemSize.y % 2 == 0);
            }
        }
        else
        {
            //swap with selected Item
            if (slot.isOccupied)
            {
                if (this.CanEquipItem(selectedItem, slot))
                {
                    var tempItem = slot.Item;
                    //unequip
                    InventoryEvents.current.UnEquipItem(slot, slot.Item);

                    //equip
                    slot.Item = selectedItem;
                    InventoryEvents.current.EquipItem(slot, slot.Item);

                    selectedItem = tempItem;
                    selectedFrom = this;
                    this.HighlightEquipmentSlot(slot, Colors.Blue);
                    this.SetSlotRaycastTartget(selectedItem.itemSize.x % 2 == 0, selectedItem.itemSize.y % 2 == 0);
                }
            }
            //drop the selected item in the slot if the type matches
            else
            {
                if (this.CanEquipItem(selectedItem, slot))
                {
                    slot.Item = selectedItem;
                    slot.isOccupied = true;
                    InventoryEvents.current.EquipItem(slot, selectedItem);
                    this.HighlightEquipmentSlot(slot, Colors.Blue);
                    this.SetSlotRaycastTartget(false, false);
                    isItemSelected = false;
                    selectedItem = null;
                    selectedFrom = null;
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
                this.HighlightEquipmentSlot(slot, Colors.Green);
            }
        }
        else
        {
            if (this.CanEquipItem(selectedItem, slot))
            {
                this.HighlightEquipmentSlot(slot, Colors.Green);
            }
            else
            {
                this.HighlightEquipmentSlot(slot, Colors.Red);
            }
        }
    }

    private bool CanEquipItem(BaseItem selectedItem, EquipmentSlot slot)
    {
        return selectedItem.ItemType == slot.SlotType;
    }

}