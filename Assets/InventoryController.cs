using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory PlayerInventory;
    
    void Start()
    {
        AddItemToInventory();
    }

    private void AddItemToInventory()
    {
        var item = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 2,
                y = 3
            }
        };

        var item2 = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 1,
                y = 1
            }
        };

        var slot = this.PlayerInventory.GetFirstAvailableSlotForItem(item);
        this.PlayerInventory.AddItem(slot.slotPosition, item);
        this.PlayerInventory.AddItem(new Vector2Int(3,0), item2);

    }
}

public class TempItem : InventoryItem
{

}
