using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents : MonoBehaviour
{
    public static InventoryEvents current;
    private void Start()
    {
        current = this;
    }
    
    public event Action<InventorySlot> onPointerEnterSlot;
    public void PointerEnterSlot(InventorySlot slot)
    {
        this.onPointerEnterSlot?.Invoke(slot);
    }

    public event Action<InventorySlot> onPointerExitSlot;
    public void PointerExitSlot(InventorySlot slot)
    {
        this.onPointerExitSlot?.Invoke(slot);
    }

    public event Action<InventorySlot> onPointerClickSlot;
    public void PointerClickSlot(InventorySlot slot)
    {
        this.onPointerClickSlot?.Invoke(slot);
    }
}
