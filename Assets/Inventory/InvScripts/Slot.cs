using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*public item slotItem;*/
    public int slotID; // The ID of the slot

    public Image slotImage;
    public Text slotNum;
    public string slotInfo;

    public GameObject itemInSlot; // The item in the slot

    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
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
    }
}
