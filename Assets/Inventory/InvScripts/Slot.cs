using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    /*public item slotItem;*/
    public int slotID; // The ID of the slot

    public string slotItemName; // The name of the item in the slot
    public bool slotItemEquiptable; // If the item in the slot is equiptable
    public int slotItemAmount; // The amount of the item in the slot

    public Image slotImage;
    public Text slotNum;
    public string slotInfo;

    public Sprite nullItemSlotImage;

    public GameObject itemInSlot; // The item in the slot

    Image slotBackground;

    // Static variable to keep track of the currently selected slot
    private static Slot currentlySelectedSlot = null;

    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo, slotItemName, slotItemEquiptable, slotItemAmount);

        // Reset the color of the previously selected slot
        if (currentlySelectedSlot != null && currentlySelectedSlot != this)
        {
            currentlySelectedSlot.slotBackground.color = Color.white;
        }

        currentlySelectedSlot = this;

        /*Debug.Log("Slot clicked: " + slotID);*/

        slotBackground = GetComponent<Image>();
        slotBackground.color = new Color(0f, 245f, 170f, 255f);
    }

    public void SetUpSlot(item item)
    {
        // if there is no item in the slot, then set the item to inactive
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

        slotItemAmount = item.item_amount;
    }

    public void ClearSlot()
    {
        // clear the slot
        slotImage.sprite = nullItemSlotImage;
        slotNum.text = "";
        slotInfo = "";

        slotItemName = "";
        slotItemEquiptable = false;
    }
}
