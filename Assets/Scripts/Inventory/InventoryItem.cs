using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public BaseItem item;
    public GameObject ItemTooltip;
    private bool isSelected;

    // Start is called before the first frame update
    private void Start()
    {
        InventoryEvents.current.onItemSelected += ItemSelected;
        InventoryEvents.current.onItemDropped += ItemDropped;
        InventoryEvents.current.onItemEquipped += ItemEquipped;
        InventoryEvents.current.onItemUnEquipped += ItemUnEquipped;        
        InventoryEvents.current.onPointerEnterItem += ShowItemTooltip;
        InventoryEvents.current.onPointerExitItem += HideItemTooltip;
    }

    private void Update()
    {
        if (this.isSelected)
        {
            this.transform.position = new Vector3(Input.mousePosition.x - this.GetComponent<RectTransform>().rect.width/2, Input.mousePosition.y + this.GetComponent<RectTransform>().rect.height/2, 0);
        }
    }

    internal void SetInventoryItem(BaseItem item, InventorySlot slot, GameObject tooltip)
    {
        this.item = item;
        this.ItemTooltip = tooltip;
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.itemSize.x * 48);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.itemSize.y * 48);
        this.GetComponent<Image>().sprite = item.image;
        this.transform.SetParent(slot.transform.parent);
        this.transform.SetAsLastSibling();
        this.transform.localPosition = new Vector3(slot.transform.localPosition.x - 24 , slot.transform.localPosition.y + 24, 0);
    }

    private void ItemSelected(BaseItem item)
    {
        if (this.item == item)
        {            
            this.isSelected = true;
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory Canvas").transform);
        }
    }

    private void ItemDropped(BaseItem item, InventorySlot slot)
    {
        if(this.item == item)
        {
            this.isSelected = false;
            this.transform.SetParent(slot.transform.parent);
            this.transform.SetAsLastSibling();
            this.transform.localPosition = new Vector3(slot.transform.localPosition.x - 24, slot.transform.localPosition.y + 24, 0);
        }
    }    

    private void ItemUnEquipped(EquipmentSlot slot, EquipableItem item)
    {
        if (this.item == item)
        {            
            this.isSelected = true;
            this.transform.SetParent(GameObject.FindGameObjectWithTag("Inventory Canvas").transform);
        }
    }

    private void ItemEquipped(EquipmentSlot slot, EquipableItem item)
    {
        if(this.item == item)
        {
            this.isSelected = false;
            this.transform.SetParent(slot.transform);
            this.transform.SetAsLastSibling();
            this.transform.localPosition = new Vector3((- slot.transform.GetComponent<RectTransform>().rect.width / 2) + (slot.transform.GetComponent<RectTransform>().rect.width - this.GetComponent<RectTransform>().rect.width)/2
                                                        ,(slot.transform.GetComponent<RectTransform>().rect.height / 2) - (slot.transform.GetComponent<RectTransform>().rect.height - this.GetComponent<RectTransform>().rect.height)/2
                                                        ,0);
        }
    }

    
    //Item tooltip
    private void ShowItemTooltip(BaseItem item)
    {
        if(this.item == item)
        {
            ItemTooltip.SetActive(true);            
            ItemTooltip.GetComponent<ItemTooltip>().SetItem(this);
        }
    }

    private void HideItemTooltip(BaseItem item)
    {
        if(this.item == item)
        {
            ItemTooltip.SetActive(false);
        }
    }
}
