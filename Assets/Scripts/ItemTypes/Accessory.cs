using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Accesory", menuName = "Items/Equipable Item/Accessory")]
public class Accessory : EquipableItem
{
    public Accessory() : base()
    {
    }

    public Accessory(Accessory accessory) : base (accessory)
    {
        
    }
}
