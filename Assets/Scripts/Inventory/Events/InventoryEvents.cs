using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents : MonoBehaviour
{
    public static InventoryEvents current;

    private void Awake()
    {
        current = this;
    }
    
    public event Action<InventorySlot> onPointerEnterInventorySlot;
    public event Action<EquipmentSlot> onPointerEnterEquipmentSlot;

    public void PointerEnterSlot(object slot)
    {
        if(slot.GetType() == typeof(InventorySlot))
        {
            this.onPointerEnterInventorySlot?.Invoke((InventorySlot)slot);
        }
        else if(slot.GetType() == typeof(EquipmentSlot))
        {
            this.onPointerEnterEquipmentSlot?.Invoke((EquipmentSlot)slot);
        }
    }

    public event Action<InventorySlot> onPointerExitInventorySlot;
    public event Action<EquipmentSlot> onPointerExitEquipmentSlot;

    public void PointerExitSlot(object slot)
    {
        if(slot.GetType() == typeof(InventorySlot))
        {
            this.onPointerExitInventorySlot?.Invoke((InventorySlot)slot);
        }
        else if(slot.GetType() == typeof(EquipmentSlot))
        {
            this.onPointerExitEquipmentSlot?.Invoke((EquipmentSlot)slot);
        }
    }

    public event Action<InventorySlot> onPointerClickInventorySlot;
    public event Action<EquipmentSlot> onPointerClickEquipmentSlot;

    public void PointerClickSlot(object slot)
    {
        if(slot.GetType() == typeof(InventorySlot))
        {
            this.onPointerClickInventorySlot?.Invoke((InventorySlot)slot);
        }
        else if(slot.GetType() == typeof(EquipmentSlot))
        {
            this.onPointerClickEquipmentSlot?.Invoke((EquipmentSlot)slot);
        }
    }

    public event Action<BaseItem> onItemSelected;  
    public void SelectItem(BaseItem item)
    {
        this.onItemSelected?.Invoke(item);
    }  

    public event Action<BaseItem, InventorySlot> onItemDropped;  
    public void DropItem(BaseItem item, InventorySlot slot)
    {
        this.onItemDropped?.Invoke(item,slot);
    }

    public event Action<EquipmentSlot, EquipableItem> onItemEquipped;
    public void EquipItem(EquipmentSlot slot, EquipableItem item)
    {
        this.onItemEquipped?.Invoke(slot,item);
    }

    public event Action<EquipmentSlot, EquipableItem> onItemUnEquipped;
    public void UnEquipItem(EquipmentSlot slot, EquipableItem item)
    {
        this.onItemUnEquipped?.Invoke(slot,item);
    }

    public event Action<BaseItem> onPointerEnterItem;
    public void PointerEnterItem(BaseItem item)
    {
        this.onPointerEnterItem?.Invoke(item);
    }

    public event Action<BaseItem> onPointerExitItem;
    public void PointerExitItem(BaseItem item)
    {
        this.onPointerExitItem?.Invoke(item);
    }
}
