using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemValue;
    [SerializeField] private TextMeshProUGUI levelRequirement;


    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        this.itemName.text = inventoryItem.item.itemName;

        if (inventoryItem.item.GetType() == typeof(Weapon))
        {
            this.itemValue.gameObject.SetActive(true);
            Weapon item = (Weapon)inventoryItem.item;
            string weaponType = Regex.Replace(Enum.GetName(typeof(WeaponType),item.weaponType), "[A-Z]"," $0");
            this.itemValue.text = weaponType + " \n" +  "Damage: "+ item.minDamage + " - " + item.maxDamage;
        }
        else if (inventoryItem.item.GetType() == typeof(Armour) || inventoryItem.item.GetType().IsSubclassOf(typeof(Armour)))
        {
            this.itemValue.gameObject.SetActive(true);
            Armour item = (Armour)inventoryItem.item;
            this.itemValue.text = "Defence: " + item.defence.ToString();
            if(inventoryItem.item.GetType() == typeof(Shield))
            {
                Shield shield = (Shield)inventoryItem.item;
                this.itemValue.text = this.itemValue.text + "\n" + "Chance to Block: " + shield.blockChance + " %";
            }
        }
        else if (inventoryItem.item.GetType() == typeof(Accessory))
        {
            this.itemValue.gameObject.SetActive(false);
        }
        else
        {
            this.itemValue.gameObject.SetActive(false);
        }

        if(inventoryItem.item.GetType().IsSubclassOf(typeof(EquipableItem)))
        {           
            
            this.levelRequirement.gameObject.SetActive(true); 
            var item = (EquipableItem)inventoryItem.item;
            this.levelRequirement.text = "Required Level: " + item.requiredLevel;
        }
        else
        {
            this.levelRequirement.gameObject.SetActive(false);
        }
        
        GetComponentsInChildren<VerticalLayoutGroup>().ToList().ForEach(x=> LayoutRebuilder.ForceRebuildLayoutImmediate(x.transform.GetComponent<RectTransform>()));
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        float xPosition,yPosition;
        if(inventoryItem.transform.position.x + inventoryItem.GetComponent<RectTransform>().rect.width / 2 + this.GetComponent<RectTransform>().rect.width / 2 > Screen.width)
        {
            //this.transform.position = new Vector3(inventoryItem.transform.position.x - (inventoryItem.transform.position.x + this.GetComponent<RectTransform>().rect.width / 2 - Screen.width), inventoryItem.transform.position.y, inventoryItem.transform.position.z);
            xPosition = inventoryItem.transform.position.x - (inventoryItem.transform.position.x + this.GetComponent<RectTransform>().rect.width / 2 - Screen.width);
        }
        else if (inventoryItem.transform.position.x + inventoryItem.GetComponent<RectTransform>().rect.width / 2 - this.GetComponent<RectTransform>().rect.width / 2 < 0)
        {
            //this.transform.position = new Vector3(inventoryItem.transform.position.x + (0 - (inventoryItem.transform.position.x - this.GetComponent<RectTransform>().rect.width / 2)), inventoryItem.transform.position.y, inventoryItem.transform.position.z);
            xPosition =  inventoryItem.transform.position.x + (0 - (inventoryItem.transform.position.x - this.GetComponent<RectTransform>().rect.width / 2));
        }
        else
        {
            xPosition = inventoryItem.transform.position.x + inventoryItem.GetComponent<RectTransform>().rect.width / 2;
        }

        if(inventoryItem.transform.position.y + this.GetComponent<RectTransform>().rect.size.y > Screen.height)
        {
            yPosition = inventoryItem.transform.position.y - inventoryItem.GetComponent<RectTransform>().rect.size.y - this.GetComponent<RectTransform>().rect.size.y;
        }
        else
        {
            yPosition = inventoryItem.transform.position.y;
        }

        this.transform.position =  new Vector3(xPosition,yPosition,inventoryItem.transform.position.z);
    }
}
