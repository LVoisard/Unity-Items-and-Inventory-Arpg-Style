using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quiver", menuName = "Items/Equipable Item/Quiver")]
public class Quiver : EquipableItem
{
    public Quiver() : base()
    {
    }

    public Quiver(Quiver quiver) : base(quiver)
    {
    }
}
