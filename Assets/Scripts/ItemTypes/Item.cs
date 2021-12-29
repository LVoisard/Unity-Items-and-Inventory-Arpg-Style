using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : BaseItem
{
    public Item()
    { 
    }

    public Item(Item item)
    {
        this.itemName = item.name;
        this.itemSize = item.itemSize;
        this.dropSound = item.dropSound;
        this.image = item.image;
    }
}

public enum WeaponType
{
    OneHandedSword,
    TwoHandedSword,
    OneHandedAxe,
    TwoHandedAxe,
    Dagger,
    Bow,
}
