using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory PlayerInventory;
    
    void Start()
    {
        
    }

    public void AddItemsToInventory()
    {
        var item = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 2,
                y = 3
            },
            ItemType = EquipmentSlotType.Body,
            imagePath = "Item Icons\\Armour\\Body\\Leather Armour"
        };

        var item2 = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 1,
                y = 1
            },
            ItemType = EquipmentSlotType.Amulet,
            imagePath = "Item Icons\\Accessory\\Amulet\\Jade Amulet"
        };

        var item3 = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 1,
                y = 1
            },
            ItemType = EquipmentSlotType.Ring,
            imagePath = "Item Icons\\Accessory\\Ring\\Ruby Ring"
        };

        var item4 = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 2,
                y = 2
            },
            ItemType = EquipmentSlotType.Head,
            imagePath = "Item Icons\\Armour\\Helmet\\Soldier Helmet"
        };

        var item5 = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 2,
                y = 4
            },
            ItemType = EquipmentSlotType.Hands,
            imagePath = "Item Icons\\Weapon\\Sword\\Claymore"
        };
        var item6 = new TempItem
        {
            itemSize = new Vector2Int
            {
                x = 1,
                y = 3
            },
            ItemType = EquipmentSlotType.Hands,
            imagePath = "Item Icons\\Weapon\\Sword\\Rusted Blade"
        };

        this.PlayerInventory.AddItem(this.PlayerInventory.GetFirstAvailableSlotForItem(item).slotPosition, item);
        this.PlayerInventory.AddItem(this.PlayerInventory.GetFirstAvailableSlotForItem(item).slotPosition, item2);
        this.PlayerInventory.AddItem(this.PlayerInventory.GetFirstAvailableSlotForItem(item).slotPosition, item3);
        this.PlayerInventory.AddItem(this.PlayerInventory.GetFirstAvailableSlotForItem(item).slotPosition, item4);
        this.PlayerInventory.AddItem(this.PlayerInventory.GetFirstAvailableSlotForItem(item).slotPosition, item5);
        this.PlayerInventory.AddItem(this.PlayerInventory.GetFirstAvailableSlotForItem(item).slotPosition, item6);

    }
}

public class TempItem : BaseItem
{
    
}
