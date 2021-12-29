using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Armour", menuName = "Items/Equipable Item/Armour/Armour")]
public class Armour : EquipableItem
{
    public int defence;

    public Armour() : base()
    {
    }

    public Armour(Armour armour) : base (armour)
    {        
        this.defence = armour.defence;
    }
}
