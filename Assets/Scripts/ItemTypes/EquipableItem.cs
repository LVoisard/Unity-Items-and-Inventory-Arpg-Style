using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Equipable Item", menuName = "Items/Equipable Item/Equipable Item")]
public class EquipableItem : Item
{
    public int requiredLevel;
    public EquipmentSlotType itemType;

    public EquipableItem() : base()
    {        
    }

    public EquipableItem(EquipableItem equipableItem) : base(equipableItem)
    {
        this.requiredLevel = equipableItem.requiredLevel;        
        this.itemType = equipableItem.itemType;
    }
}
