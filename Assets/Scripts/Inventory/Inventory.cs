using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryGridContainer;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private GameObject itemTooltip;
    [SerializeField] private int rowCount;
    [SerializeField] private int colCount;

    public static bool isItemSelected = false;
    public static BaseItem selectedItem;
    public static Inventory selectedFrom;

    public bool debugIsItemSelected;

    public InventorySlot[,] inventorySlots;

    public IDictionary<BaseItem, Vector2Int> Items = new Dictionary<BaseItem, Vector2Int>();
    public List<string> itemsName = new List<string>();

    protected void Awake()
    {
        this.inventorySlots = new InventorySlot[rowCount, colCount];

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                print(transform.name);
                var slot = GameObject.Instantiate(this.inventorySlotPrefab, this.inventoryGridContainer.transform).GetComponent<InventorySlot>();
                slot.SetName($"{x},{y}");
                slot.slotPosition = new Vector2Int(x, y);
                inventorySlots[y, x] = slot;
            }
        }
    }

    protected void Start()
    {
        //Inventory slots events
        InventoryEvents.current.onPointerEnterInventorySlot += this.InventorySlot_SlotEntered;
        InventoryEvents.current.onPointerExitInventorySlot += this.InventorySlot_SlotExited;
        InventoryEvents.current.onPointerClickInventorySlot += this.InventorySlot_SlotClicked;

        InventoryEvents.current.onItemSelected += this.ItemSelectedFromInventory;
        InventoryEvents.current.onItemDropped += this.ItemDroppedInInventory;

    }

    protected void Update()
    {
        this.debugIsItemSelected = isItemSelected;
        this.itemsName = Items.Select(x => x.Key.ToString()).ToList();
    }

    public void AddItem(Vector2Int slotPosition, BaseItem item)
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
        inventoryItem.GetComponent<InventoryItem>().SetInventoryItem(item, slot, itemTooltip);

        this.PlaceItem(item, slot.slotPosition);
    }

    protected void PlaceItem(BaseItem item, Vector2Int slotPosition)
    {
        this.Items.Add(item, slotPosition);
    }

    protected void RemoveItem(BaseItem item)
    {
        this.Items.Remove(item);
    }

    protected void ItemSelectedFromInventory(BaseItem item) 
    {
        if(this.Items.Any(x => x.Key == item))
            RemoveItem(item);
    }

    protected void ItemDroppedInInventory(BaseItem item, InventorySlot slot) 
    {
        if (colCount <= slot.slotPosition.x 
            || rowCount <= slot.slotPosition.y
            || (InventorySlot)inventorySlots.GetValue(slot.slotPosition.y, slot.slotPosition.x) != slot)
        {
            return;
        }        
        
        PlaceItem(item,slot.slotPosition);
    }

    public InventorySlot GetFirstAvailableSlotForItem(BaseItem item)
    {
        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                if(x < 0 || y < 0 || x >= colCount || y >= rowCount)
                {
                    continue;
                }
                if (!this.inventorySlots[y, x].isOccupied && !slotUnderItemAreOccupied(this.inventorySlots[y, x], item))
                {
                    return this.inventorySlots[y, x];
                }
            }
        }
        return null;
    }

    protected void HighlightItem(InventorySlot slot, BaseItem item, Color color)
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

    protected void UnHightlightItem(InventorySlot slot, BaseItem item)
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

    protected void HighlightSlots(InventorySlot slot, BaseItem item, Color color)
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

    protected void UnHighlightSlots(InventorySlot slot, BaseItem item)
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

    protected bool slotUnderItemAreOccupied(InventorySlot slot, BaseItem item)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {                
                if(x < 0 || y < 0 || x >= colCount || y >= rowCount)
                {
                    return true;
                }
                if (inventorySlots[y, x].isOccupied)
                {
                    print("occupied at: " + inventorySlots[y,x].name);
                    return true;
                }
            }
        }
        return false;
    }

    protected void InventorySlot_SlotEntered(InventorySlot slot)
    {
        if (colCount <= slot.slotPosition.x 
            || rowCount <= slot.slotPosition.y
            || (InventorySlot)inventorySlots.GetValue(slot.slotPosition.y, slot.slotPosition.x) != slot)
        {
            return;
        }

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
                    InventoryEvents.current.PointerEnterItem(parentSlot.Item);
                }
            }
        }
        else
        {
            var offsetSlot = this.inventorySlots[slot.slotPosition.y - selectedItem.itemSize.y / 2, slot.slotPosition.x];
            var parentSlots = this.ItemCountUnderItem(offsetSlot, selectedItem);
            if (parentSlots == null)
            {
                Debug.LogWarning("Error: Index out of range");
            }
            else if (parentSlots.Count == 0)
            {
                this.HighlightSlots(offsetSlot, selectedItem, Colors.Blue);
            }
            else if (parentSlots.Count == 1)
            {
                this.HighlightSlots(parentSlots[0], parentSlots[0].Item, Colors.Green);
            }
            else
            {
                this.HighlightSlots(offsetSlot, selectedItem, Colors.Red);
            }
        }
    }

    protected void InventorySlot_SlotExited(InventorySlot slot)
    {
        if (colCount <= slot.slotPosition.x 
            || rowCount <= slot.slotPosition.y
            || (InventorySlot)inventorySlots.GetValue(slot.slotPosition.y, slot.slotPosition.x) != slot)
        {
            return;
        }

        if (!isItemSelected)
        {
            if (slot.isOccupied)
            {
                //when the slot contains an item and no item is selected we want to highlight all the slot containing the item to the original colour
                if (slot.Item != null)
                {
                    //take the slot's parent, children slots also include the parent, maybe change names so it makes more sense
                    var parentSlot = slot.parentSlot ?? slot;
                    foreach (var s in parentSlot.childrenSlots)
                    {
                        s.GetComponent<Image>().color = Colors.Blue;
                    }
                    InventoryEvents.current.PointerExitItem(parentSlot.Item);
                }
            }
        }
        else
        {
            var offsetSlot = this.inventorySlots[slot.slotPosition.y - selectedItem.itemSize.y / 2, slot.slotPosition.x];
            var parentSlots = this.ItemCountUnderItem(offsetSlot, selectedItem);
            if (parentSlots == null)
            {
                Debug.LogWarning("Error: Index out of range");
            }
            else if (parentSlots.Count == 0)
            {
                this.UnHighlightSlots(offsetSlot, selectedItem);
            }
            else if (parentSlots.Count == 1)
            {
                this.HighlightItem(parentSlots[0], parentSlots[0].Item, Colors.Blue);
            }
            else
            {
                this.UnHighlightSlots(offsetSlot, selectedItem);
                foreach (var s in parentSlots)
                {
                    this.HighlightSlots(s, s.Item, Colors.Blue);
                }
            }
        }
    }

    protected void InventorySlot_SlotClicked(InventorySlot slot)
    {
        if (colCount <= slot.slotPosition.x 
            || rowCount <= slot.slotPosition.y
            || (InventorySlot)inventorySlots.GetValue(slot.slotPosition.y, slot.slotPosition.x) != slot)
        {
            return;
        }
        
        if (!isItemSelected)
        {
            //Pick Up
            if (slot.isOccupied && slot.Item != null)
            {
                InventoryEvents.current.PointerExitItem(slot.Item);
                this.SelectItem(slot.Item);
                this.SetSlotRaycastTartget(slot.Item.itemSize.x % 2 == 0, selectedItem.itemSize.y % 2 == 0);
                this.HighlightSlots(slot.parentSlot, selectedItem, Colors.Blue);
                //setup event for when an item is picked up (displaying physical item only i think)
                this.UnsetOccupiedSlots(slot.parentSlot);
            }
        }
        else
        {
            var offsetSlot = this.inventorySlots[slot.slotPosition.y - selectedItem.itemSize.y / 2, slot.slotPosition.x];
            var parentSlots = this.ItemCountUnderItem(offsetSlot, selectedItem);

            //Drop
            if (parentSlots.Count == 0)
            {
                this.DropItem(offsetSlot, selectedItem);
                selectedItem = null;
                isItemSelected = false;
                selectedFrom = null;
                this.SetSlotRaycastTartget(false, false);
                InventoryEvents.current.PointerEnterItem(slot.Item);
            }
            //Swap
            else if (parentSlots.Count == 1)
            {
                var tempItem = parentSlots[0].Item;
                this.UnsetOccupiedSlots(parentSlots[0]);
                this.DropItem(offsetSlot, selectedItem);
                this.SelectItem(tempItem);
                this.SetSlotRaycastTartget(selectedItem.itemSize.x % 2 == 0, selectedItem.itemSize.y % 2 == 0);
            }
            //can't do anything
            else
            {
                print("Cannot Drop or Swap Item");
            }
        }
    }
    
    protected List<InventorySlot> ItemCountUnderItem(InventorySlot slot, BaseItem item)
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

    protected void SelectItem(BaseItem item)
    {
        selectedItem = item;
        isItemSelected = true;
        selectedFrom = this;
        InventoryEvents.current.SelectItem(item);
    }

    protected void DropItem(InventorySlot slot, BaseItem item)
    {
        for (int y = slot.slotPosition.y; y < slot.slotPosition.y + item.itemSize.y; y++)
        {
            for (int x = slot.slotPosition.x; x < slot.slotPosition.x + item.itemSize.x; x++)
            {
                inventorySlots[y, x].isOccupied = true;
                inventorySlots[y, x].Item = item;
                inventorySlots[y, x].parentSlot = slot;
                inventorySlots[y, x].GetComponent<Image>().color = Colors.Green;
                slot.childrenSlots.Add(inventorySlots[y, x]);
            }
        }
        InventoryEvents.current.DropItem(item, slot);
    }

    protected void UnsetOccupiedSlots(InventorySlot slot)
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

    protected void SetSlotRaycastTartget(bool isItemSizeXEven, bool isItemSizeYEven)
    {
        foreach (var inventory in GameObject.FindObjectsOfType<Inventory>())
        {
            foreach (var slot in inventory.inventorySlots)
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
}
