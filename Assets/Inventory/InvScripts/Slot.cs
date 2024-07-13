using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*public item slotItem;*/
    public int slotID; // The ID of the slot

    public string slotItemName; // The name of the item in the slot
    public string selectedItemName; // The name of the selected item

    public bool slotItemEquiptable; // If the item in the slot is equiptable
    public bool selectItemEquiptable; // If the selected item is equiptable

    public Image slotImage;
    public Text slotNum;
    public string slotInfo;

    public GameObject itemInSlot; // The item in the slot

    public GameObject useButton;
    bool useButtonActive;

    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo, slotItemName, slotItemEquiptable);
    }

    public void SetUpSlot(item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotImage.sprite = item.item_icon;
        slotNum.text = item.item_amount.ToString();
        slotInfo = item.item_description;

        slotItemName = item.itemName;
        slotItemEquiptable = item.equiptable;
    }
}
