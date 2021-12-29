using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public BaseItem Item { get; set; }
    public bool isOccupied = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryEvents.current.PointerEnterSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryEvents.current.PointerExitSlot(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryEvents.current.PointerClickSlot(this);
        }        
    }
}
