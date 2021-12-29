using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Equipable Item/Weapon")]
public class Weapon : EquipableItem
{
    public float minDamage;
    public float maxDamage;
    public WeaponType weaponType;

    public Weapon() : base()
    {
    }

    public Weapon(Weapon weapon) : base(weapon)
    {
        this.weaponType = weapon.weaponType;
        this.minDamage = weapon.minDamage;
        this.maxDamage = weapon.maxDamage;        
    }
}
