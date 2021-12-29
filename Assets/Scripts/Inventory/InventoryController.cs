using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory PlayerInventory;
    [SerializeField] private AudioSource source;

    void Start()
    {
        InventoryEvents.current.onItemSelected += playPickup;
        InventoryEvents.current.onItemDropped += playDropped;
        InventoryEvents.current.onItemUnEquipped += playPickup;
        InventoryEvents.current.onItemEquipped += playDropped;
    }

    private void playPickup(BaseItem item)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds\\invgrab");
        source.PlayOneShot(clip);
    }

    private void playDropped(BaseItem item, InventorySlot slot)
    {
        AudioClip clip = item.dropSound;
        source.PlayOneShot(clip);
    }

    private void playPickup(EquipmentSlot slot, EquipableItem item)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds\\invgrab");
        source.PlayOneShot(clip);
    }

    private void playDropped(EquipmentSlot slot, EquipableItem item)
    {
        AudioClip clip = item.dropSound;
        source.PlayOneShot(clip);
    }    

    public void AddItem(Item item)
    {
        var slot = this.PlayerInventory.GetFirstAvailableSlotForItem(item);
        if(slot != null)
        {            
            this.PlayerInventory.AddItem(slot.slotPosition, item);
        }
        else
        {
            print("No more space in inventory");
        }
    }
}