using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private int rowCount;
    [SerializeField] private int colCount;

    public bool isItemSelected = false;
    public BaseItem selectedItem;

    [SerializeField]
    public InventorySlot[,] inventorySlots;

    void Awake()
    {
        this.inventorySlots = new InventorySlot[rowCount, colCount];

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                var slot = GameObject.Instantiate(this.inventorySlotPrefab, this.transform).GetComponent<InventorySlot>();
                slot.SetName($"{x},{y}");
                slot.slotPosition = new Vector2Int(x, y);
                inventorySlots[y, x] = slot;
            }
        }
    }

    private void Start()
    {
        //Inventory slots events
        InventoryEvents.current.onPointerEnterInventorySlot += this.InventorySlot_SlotEntered;
        InventoryEvents.current.onPointerExitInventorySlot += this.InventorySlot_SlotExited;
        InventoryEvents.current.onPointerClickInventorySlot += this.InventorySlot_SlotClicked;

        //Equipment slots events
        InventoryEvents.current.onPointerEnterEquipmentSlot += this.EquipmentSlot_SlotEntered;
        InventoryEvents.current.onPointerExitEquipmentSlot += this.EquipmentSlot_SlotExited;
        InventoryEvents.current.onPointerClickEquipmentSlot += this.EquipmentSlot_SlotClicked;
    }

    public void AddItem(Vector2Int slotPosition, TempItem item)
    {
        var slot = inventorySlots[slotPosition.y, slotPosition.x];
        slot.Item = item;

        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                inventorySlots[y, x].isOccupied = true;
                inventorySlots[y, x].Item = item;
                inventorySlots[y, x].parentSlot = slot;
                inventorySlots[y, x].GetComponent<Image>().color = Colors.Blue;
                slot.childrenSlots.Add(inventorySlots[y, x]);
            }
        }

        GameObject inventoryItem = Instantiate(this.inventoryItemPrefab, this.transform);
        inventoryItem.transform.position = new Vector3((slotPosition.x - 1) * 48 + 6.5f, (this.rowCount - slotPosition.y) * 48 + 5f, 0);
        inventoryItem.GetComponent<InventoryItem>().SetInventoryItem(item);
    }

    public InventorySlot GetFirstAvailableSlotForItem(BaseItem item)
    {
        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                if (!this.inventorySlots[y, x].isOccupied && !slotUnderItemAreOccupied(this.inventorySlots[y, x], item))
                {
                    return this.inventorySlots[y, x];
                }
            }
        }
        return null;
    }

    private void HighlightItem(InventorySlot slot, BaseItem item, Color color)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                if (x < 0 || y < 0 || x >= this.colCount || y >= this.rowCount)
                    return;
                inventorySlots[y, x].GetComponent<Image>().color = color;
            }
        }
    }

    private void UnHightlightItem(InventorySlot slot, BaseItem item)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                if (x < 0 || y < 0 || x >= this.colCount || y >= this.rowCount)
                    return;
                inventorySlots[y, x].GetComponent<Image>().color = Colors.Clear;
            }
        }
    }

    private void HighlightSlots(InventorySlot slot, BaseItem item, Color color)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                if (x < 0 || y < 0 || x >= this.colCount || y >= this.rowCount)
                    return;
                inventorySlots[y, x].GetComponent<Image>().color = color;
            }
        }
    }

    private void UnHighlightSlots(InventorySlot slot, BaseItem item)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                if (x < 0 || y < 0 || x >= this.colCount || y >= this.rowCount)
                    return;
                inventorySlots[y, x].GetComponent<Image>().color = Colors.Clear;
            }
        }
    }

    private bool slotUnderItemAreOccupied(InventorySlot slot, BaseItem item)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                if (inventorySlots[y, x].isOccupied)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void InventorySlot_SlotEntered(InventorySlot slot)
    {
        if (!isItemSelected)
        {
            if (slot.isOccupied)
            {
                //when the slot contains an item and no item is selected we want to highlight all the slot containing the item
                if (slot.Item != null)
                {
                    //take the slot's parent
                    var parentSlot = slot.parentSlot ?? slot;
                    this.HighlightItem(parentSlot, parentSlot.Item, Colors.Green);
                }
            }
        }
        else
        {
            var offsetSlot = this.inventorySlots[slot.slotPosition.y - this.selectedItem.itemSize.y / 2, slot.slotPosition.x];
            var parentSlots = this.ItemCountUnderItem(offsetSlot, this.selectedItem);
            if (parentSlots == null)
            {
                Debug.LogWarning("Error: Index out of range");
            }
            else if (parentSlots.Count == 0)
            {
                this.HighlightSlots(offsetSlot, this.selectedItem, Colors.Blue);
            }
            else if (parentSlots.Count == 1)
            {
                this.HighlightSlots(parentSlots[0], parentSlots[0].Item, Colors.Green);
            }
            else
            {
                this.HighlightSlots(offsetSlot, this.selectedItem, Colors.Red);
            }
        }
    }

    private void InventorySlot_SlotExited(InventorySlot slot)
    {
        if (!isItemSelected)
        {
            if (slot.isOccupied)
            {
                //when the slot contains an item and no item is selected we want to highlight all the slot containing the item
                if (slot.Item != null)
                {
                    //take the slot's parent, children slots also include the parent, maybe change names so it makes more sense
                    var parentSlot = slot.parentSlot ?? slot;
                    foreach (var s in parentSlot.childrenSlots)
                    {
                        s.GetComponent<Image>().color = Colors.Blue;
                    }
                }
            }
        }
        else
        {
            var offsetSlot = this.inventorySlots[slot.slotPosition.y - this.selectedItem.itemSize.y / 2, slot.slotPosition.x];
            var parentSlots = this.ItemCountUnderItem(offsetSlot, this.selectedItem);
            if (parentSlots == null)
            {
                Debug.LogWarning("Error: Index out of range");
            }
            else if (parentSlots.Count == 0)
            {
                this.UnHighlightSlots(offsetSlot, this.selectedItem);
            }
            else if (parentSlots.Count == 1)
            {
                this.HighlightItem(parentSlots[0], parentSlots[0].Item, Colors.Blue);
            }
            else
            {
                this.UnHighlightSlots(offsetSlot, this.selectedItem);
                foreach (var s in parentSlots)
                {
                    this.HighlightSlots(s, s.Item, Colors.Blue);
                }
            }
        }
    }

    private void InventorySlot_SlotClicked(InventorySlot slot)
    {
        print(slot.slotPosition.x + "," + slot.slotPosition.y);

        if (!isItemSelected)
        {
            //Pick Up
            if (slot.isOccupied && slot.Item is object)
            {
                this.SelectItem(slot.Item);
                this.SetSlotRaycastTartget(slot.Item.itemSize.x % 2 == 0, this.selectedItem.itemSize.y % 2 == 0);
                this.HighlightSlots(slot.parentSlot,this.selectedItem,Colors.Blue);
                //setup event for when an item is picked up (displaying physical item only i think)
                this.UnsetOccupiedSlots(slot.parentSlot);
            }
        }
        else
        {
            var offsetSlot = this.inventorySlots[slot.slotPosition.y - this.selectedItem.itemSize.y / 2, slot.slotPosition.x];
            var parentSlots = this.ItemCountUnderItem(offsetSlot, this.selectedItem);

            //Drop
            if (parentSlots.Count == 0)
            {
                this.DropItem(offsetSlot, this.selectedItem);
                this.selectedItem = null;
                this.isItemSelected = false;
                this.SetSlotRaycastTartget(false, false);
            }
            //Swap
            else if (parentSlots.Count == 1)
            {
                var tempItem = parentSlots[0].Item;
                this.UnsetOccupiedSlots(parentSlots[0]);
                this.DropItem(offsetSlot, this.selectedItem);
                this.SelectItem(tempItem);
                this.SetSlotRaycastTartget(this.selectedItem.itemSize.x % 2 == 0, this.selectedItem.itemSize.y % 2 == 0);
            }
            //can't do anything
            else
            {
                print("Cannot Drop or Swap Item");
            }
        }
    }

    private void EquipmentSlot_SlotEntered(EquipmentSlot slot)
    {
        if (!this.isItemSelected)
        {
            if (slot.isOccupied)
            {
                this.HighlightEquipmentSlot(slot, Colors.Green);
            }
        }
        else
        {
            if (this.CanEquipItem(this.selectedItem, slot))
            {
                this.HighlightEquipmentSlot(slot, Colors.Green);
            }
            else
            {
                this.HighlightEquipmentSlot(slot, Colors.Red);
            }
        }
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
        if (!this.isItemSelected)
        {
            if (slot.isOccupied)
            {
                //pickup the item
                this.isItemSelected = true;
                this.selectedItem = slot.Item;
                this.HighlightEquipmentSlot(slot, Colors.Clear);
                InventoryEvents.current.UnEquipItem(slot, slot.Item);
                slot.Item = null;
                slot.isOccupied = false;
                this.SetSlotRaycastTartget(this.selectedItem.itemSize.x % 2 == 0, this.selectedItem.itemSize.y % 2 == 0);
            }
        }
        else
        {
            //swap with selected Item
            if (slot.isOccupied)
            {
                if (this.CanEquipItem(this.selectedItem, slot))
                {
                    var tempItem = slot.Item;
                    //unequip
                    InventoryEvents.current.UnEquipItem(slot, slot.Item);

                    //equip
                    slot.Item = this.selectedItem;
                    InventoryEvents.current.EquipItem(slot, slot.Item);

                    this.selectedItem = tempItem;
                    this.HighlightEquipmentSlot(slot, Colors.Blue);
                    this.SetSlotRaycastTartget(this.selectedItem.itemSize.x % 2 == 0, this.selectedItem.itemSize.y % 2 == 0);
                }
            }
            //drop the selected item in the slot if the type matches
            else
            {
                if (this.CanEquipItem(this.selectedItem, slot))
                {
                    slot.Item = this.selectedItem;
                    slot.isOccupied = true;
                    InventoryEvents.current.EquipItem(slot, this.selectedItem);
                    this.HighlightEquipmentSlot(slot, Colors.Blue);
                    this.SetSlotRaycastTartget(false, false);
                    this.isItemSelected = false;
                    this.selectedItem = null;
                }
            }
        }
    }

    private void HighlightEquipmentSlot(EquipmentSlot slot, Color color)
    {
        slot.GetComponent<Image>().color = color;
    }

    private bool CanEquipItem(BaseItem selectedItem, EquipmentSlot slot)
    {
        return selectedItem.ItemType == slot.SlotType;
    }


    private List<InventorySlot> ItemCountUnderItem(InventorySlot slot, BaseItem item)
    {
        var parentSlots = new List<InventorySlot>(0);
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                if (x < 0 || y < 0 || x >= this.colCount || y >= this.rowCount)
                    return null;
                if (inventorySlots[y, x].isOccupied && !parentSlots.Contains(inventorySlots[y, x].parentSlot))
                {
                    parentSlots.Add(inventorySlots[y, x].parentSlot);
                }
            }
        }
        return parentSlots;
    }

    private void SelectItem(BaseItem item)
    {
        this.selectedItem = item;
        this.isItemSelected = true;        
        InventoryEvents.current.SelectItem(item);
    }

    private void DropItem(InventorySlot slot, BaseItem item)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                inventorySlots[y, x].isOccupied = true;
                inventorySlots[y, x].Item = item;
                inventorySlots[y, x].parentSlot = slot;
                inventorySlots[y, x].GetComponent<Image>().color = Colors.Blue;
                slot.childrenSlots.Add(inventorySlots[y, x]);
            }
        }
        InventoryEvents.current.DropItem(item, slot);
    }

    private void UnsetOccupiedSlots(InventorySlot slot)
    {
        foreach (var s in slot.childrenSlots)
        {
            s.parentSlot = null;
            s.Item = null;
            s.childrenSlots = new List<InventorySlot>(0);
            s.isOccupied = false;
            s.GetComponent<Image>().color = Colors.Clear;
        }
    }

    private void SetSlotRaycastTartget(bool isItemSizeXEven, bool isItemSizeYEven)
    {
        foreach (var slot in this.inventorySlots)
        {
            if (isItemSizeXEven)
            {
                if (isItemSizeYEven)
                {
                    slot.GetComponent<Image>().raycastPadding = new Vector4(24, 24, -24, -24);
                }
                else
                {
                    slot.GetComponent<Image>().raycastPadding = new Vector4(24, 0, -24, 0);
                }
            }
            else
            {
                if (isItemSizeYEven)
                {
                    slot.GetComponent<Image>().raycastPadding = new Vector4(0, 24, 0, -24);
                }
                else
                {
                    slot.GetComponent<Image>().raycastPadding = new Vector4(0, 0, 0, 0);
                }
            }
        }
    }
}
