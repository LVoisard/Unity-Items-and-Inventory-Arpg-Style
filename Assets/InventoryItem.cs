﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public BaseItem Item;
    private bool isSelected;

    // Start is called before the first frame update
    private void Start()
    {
        InventoryEvents.current.onItemSelected += ItemSelected;
        InventoryEvents.current.onItemDropped += ItemDropped;
        InventoryEvents.current.onItemEquipped += ItemEquipped;
        InventoryEvents.current.onItemUnEquipped += ItemUnEquipped;        
    }

    private void Update()
    {
        if (this.isSelected)
        {
            this.transform.position = new Vector3(Input.mousePosition.x - ((float)this.Item.itemSize.x / 2 * 48), Input.mousePosition.y + ((float)this.Item.itemSize.y / 2 * 48), -1000);
        }
    }

    internal void SetInventoryItem(BaseItem item, InventorySlot slot)
    {
        this.Item = item;
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * 48);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * 48);
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.imagePath);
        this.transform.position = new Vector3(slot.transform.position.x - 24 , slot.transform.position.y + 24, 0);
    }

    private void ItemSelected(BaseItem item)
    {
        if (this.Item == item)
        {            
            this.isSelected = true;
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory Canvas").transform);
        }
    }

    private void ItemDropped(BaseItem item, InventorySlot slot)
    {
        if(this.Item == item)
        {
            this.isSelected = false;
            this.transform.SetParent(slot.transform.parent);
            this.transform.SetAsLastSibling();
            this.transform.localPosition = new Vector3(slot.transform.localPosition.x - 24, slot.transform.localPosition.y + 24 - 2.5f, 0);            transform.SetAsLastSibling();
        }
    }    

    private void ItemUnEquipped(EquipmentSlot slot, BaseItem item)
    {
        if (this.Item == item)
        {            
            this.isSelected = true;
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory Canvas").transform);
        }
    }

    private void ItemEquipped(EquipmentSlot slot, BaseItem item)
    {
        if(this.Item == item)
        {
            this.isSelected = false;
            this.transform.SetParent(slot.transform.parent);
            this.transform.SetAsLastSibling();
            this.transform.position = new Vector3(slot.transform.position.x - item.itemSize.x * 48 / 2, slot.transform.position.y + item.itemSize.y * 48 / 2 ,0);  
        }
    }
}