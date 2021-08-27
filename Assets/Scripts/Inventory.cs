using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private int rowCount;
    [SerializeField] private int colCount;

    public bool isItemSelected = false;
    public TempItem selectedItem;

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
        InventoryEvents.current.onPointerEnterSlot += this.InventorySlot_SlotEntered;
        InventoryEvents.current.onPointerExitSlot += this.InventorySlot_SlotExited;
        InventoryEvents.current.onPointerClickSlot += this.InventorySlot_SlotClicked;
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
    }

    public InventorySlot GetFirstAvailableSlotForItem(InventoryItem item)
    {
        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                if (!this.inventorySlots[y, x].isOccupied)
                {
                    return this.inventorySlots[y, x];
                }
            }
        }
        return null;
    }

    private void HighlightItem(InventorySlot slot, InventoryItem item, Color color)
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

    private void UnHightlightItem(InventorySlot slot, TempItem item)
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

    private void HighlightSlots(InventorySlot slot, TempItem item, Color color)
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

    private void UnHighlightSlots(InventorySlot slot, TempItem item)
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

    private bool slotUnderItemAreOccupied(InventorySlot slot, TempItem item)
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
                this.selectedItem = slot.Item;
                this.isItemSelected = true;
                this.SetSlotRaycastTartget(slot.Item.itemSize.x % 2 == 0, this.selectedItem.itemSize.y % 2 == 0);

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
                this.SetSlotRaycastTartget(false,false);
            }
            //Swap
            else if (parentSlots.Count == 1)
            {
                var tempItem = parentSlots[0].Item;
                this.UnsetOccupiedSlots(parentSlots[0]);
                this.DropItem(offsetSlot, this.selectedItem);
                this.selectedItem = tempItem;
                this.isItemSelected = true;
                this.SetSlotRaycastTartget(this.selectedItem.itemSize.x % 2 == 0, this.selectedItem.itemSize.y % 2 == 0);
            }
            //can't do anything
            else
            {
                print("Cannot Drop or Swap Item");
            }
        }
    }

    private List<InventorySlot> ItemCountUnderItem(InventorySlot slot, TempItem item)
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

    private void DropItem(InventorySlot slot, TempItem item)
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
                if(isItemSizeYEven)
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
                if(isItemSizeYEven)
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
