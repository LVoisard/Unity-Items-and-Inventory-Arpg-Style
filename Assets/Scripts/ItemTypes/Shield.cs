using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Shield", menuName = "Items/Equipable Item/Armour/Shield")]
public class Shield : Armour
{
    public int blockChance;
    
    public Shield()
    {        
    }
    
    public Shield(Shield shield) : base(shield)
    {        
        this.blockChance = shield.blockChance;
    }
}
