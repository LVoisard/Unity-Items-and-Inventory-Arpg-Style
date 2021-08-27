using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Text text;
    [SerializeField] public GameObject evenItemSlotTarget;

    public Vector2Int slotPosition = new Vector2Int(0,0);
    public bool isOccupied = false;

    public TempItem Item { get; set; }  
    public InventorySlot parentSlot { get; set; }
    public List<InventorySlot> childrenSlots { get; set; } = new List<InventorySlot>(0);

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

    public void SetName(string name)
    {
        this.name = name;
        this.text.text = name;
    }

}
