using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AllItemsMenu : MonoBehaviour
{
    [SerializeField] private GameObject AllItemsListContainer;
    [SerializeField] private GameObject ItemList;
    [SerializeField] private GameObject AddItemListItemPrefab;
    private List<GameObject> Items;
    private int CurrentDropdownValue;
    private string CurrentStringLookup;

    private void Start()
    {
        Items = new List<GameObject>();
        LoadAllItems();
    }

    public void LoadAllItems()
    {
        var items = Resources.LoadAll<Item>("Items").OrderBy(x => x.itemName).ToList();

        foreach (var item in items)
        {
            GameObject obj = Instantiate(AddItemListItemPrefab, ItemList.transform);
            AddItemListItem addItem = obj.GetComponent<AddItemListItem>();
            addItem.SetItem((Item)item);
            Items.Add(obj);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void OnDropdownValueChanged(int val)
    {
        this.CurrentDropdownValue = val;
        this.UpdateItemList();
    }

    public void OnSearchChanged(string val)
    {
        this.CurrentStringLookup = val;
        this.UpdateItemList();
    }

    private void UpdateItemList()
    {
        this.Items.ForEach(x => x.SetActive(true));
        switch (CurrentDropdownValue)
        {
            case 0:
                this.Items.ForEach(x => x.SetActive(true));
                break;
            case 1:
                this.Items.Where(x => x.GetComponent<AddItemListItem>().GetItemType() != typeof(Item)).ToList().ForEach(x => x.SetActive(false));
                break;
            case 2:
                this.Items.Where(x => x.GetComponent<AddItemListItem>().GetItemType() != typeof(Weapon) && !x.GetComponent<AddItemListItem>().GetItemType().IsSubclassOf(typeof(Weapon))).ToList().ForEach(x => x.SetActive(false));
                break;
            case 3:
                this.Items.Where(x => x.GetComponent<AddItemListItem>().GetItemType() != typeof(Armour) && !x.GetComponent<AddItemListItem>().GetItemType().IsSubclassOf(typeof(Armour))).ToList().ForEach(x => x.SetActive(false));
                break;  
            case 4:
                this.Items.Where(x => x.GetComponent<AddItemListItem>().GetItemType() != typeof(Accessory) && !x.GetComponent<AddItemListItem>().GetItemType().IsSubclassOf(typeof(Accessory))).ToList().ForEach(x => x.SetActive(false));
                break;
            case 5:
                this.Items.Where(x => x.GetComponent<AddItemListItem>().GetItemType() != typeof(Quiver) && !x.GetComponent<AddItemListItem>().GetItemType().IsSubclassOf(typeof(Quiver))).ToList().ForEach(x => x.SetActive(false));
                break;  
            case 6:
                this.Items.Where(x => x.GetComponent<AddItemListItem>().GetItemType() != typeof(Shield) && !x.GetComponent<AddItemListItem>().GetItemType().IsSubclassOf(typeof(Shield))).ToList().ForEach(x => x.SetActive(false));
                break;  
        }

        if(!string.IsNullOrEmpty(CurrentStringLookup))
        {
            this.Items.Where(x => !x.GetComponent<AddItemListItem>().GetItem().itemName.ToLower().Contains(CurrentStringLookup.ToLower())).ToList()
                .ForEach(x => x.SetActive(false));
        }
    }
}
