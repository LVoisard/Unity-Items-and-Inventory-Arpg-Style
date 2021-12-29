using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddItemListItem : MonoBehaviour
{
    private Item item;
    [SerializeField] private Image ItemIcon;
    [SerializeField] private TextMeshProUGUI ItemNameText;

    public void SetItem(Item item)
    {
        this.item = item;
        ItemIcon.sprite = item.image;
        ItemNameText.text = item.itemName;
    }

    public void AddItemToInventory()
    {
        Item newItem = item;
        
        if(item.GetType() == typeof(Shield))
        {
            newItem = new Shield((Shield)item);
        }
        else if(item.GetType() == typeof(Quiver))
        {
            newItem = new Quiver((Quiver)item);
        }
        else if(item.GetType() == typeof(Weapon))
        {
            newItem = new Weapon((Weapon)item);
        }
        else if(item.GetType() == typeof(Accessory))
        {
            newItem = new Accessory((Accessory)item);
        }
        else if(item.GetType() == typeof(Armour))
        {
            newItem = new Armour((Armour)item);
        }
        else if(item.GetType() == typeof(EquipableItem))
        {
            newItem = new EquipableItem((EquipableItem)item);
        }
        else
        {
            newItem = new Item(item);
        }

        FindObjectOfType<InventoryController>().AddItem(newItem);
    }

    public Item GetItem()
    {
        return item;
    }

    public System.Type GetItemType()
    {
        return this.item.GetType();
    }
}
