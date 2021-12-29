using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlot : Slot
{
    [SerializeField] private Text text;

    public Vector2Int slotPosition = new Vector2Int(0,0);     
    public InventorySlot parentSlot { get; set; }
    public List<InventorySlot> childrenSlots { get; set; } = new List<InventorySlot>(0);    

    public void SetName(string name)
    {
        this.name = name;
        this.text.text = name;
    }
}
