using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // Singleton

    public characterController CharacterController;

    public Inventory MyInventory;
    public GameObject slotGrid;
    /*public Slot slotPrefab;*/
    public GameObject emptySlot;
    public Text item_description;

    string item_name;
    bool item_equitable;
    int item_amount;

    int discardIndex;

    public List<GameObject> slots = new List<GameObject>(); // used to store the slots

    public GameObject projectilePrefab_fire;
    public GameObject projectilePrefab_greenFire;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        RefreshItem();
        instance.item_description.text = "";
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public static void UpdateItemInfo(string itemDescription, string itemName, bool itemEquitable, int itemAmount)
    {
        instance.item_description.text = itemDescription;

        instance.item_name = itemName;
        instance.item_equitable = itemEquitable;

        instance.item_amount = itemAmount;
    }

    // update the amount of the item
    public void UpdateItemAmount(string itemName, int newAmount)
    {
        item foundItem = MyInventory.items.Find(i => i != null && i.itemName == itemName);
        if (foundItem != null)
        {
            foundItem.item_amount = newAmount;
        }
    }

    // use the item
    public void useItem()
    {
        /*Debug.Log("using:" + item_name);*/
        if (item_equitable)
        {
            switch (item_name)
            {
                case "Fire Projectile":
                    CharacterController.changeProjectileInInventory(item_name);
                    break;
                case "Green Fire Projectile":
                    CharacterController.changeProjectileInInventory(item_name);
                    break;
                default:
                    Debug.Log("No item equipped");
                    break;
            }
        }
        else
        {
            if (item_amount > 0)
            {
                switch (item_name)
                {
                    case "health":
                        if (CharacterController.health < 5)
                        {
                            CharacterController.ChangeHealth(1);

                            item_amount--;
                            UpdateItemAmount(item_name, item_amount);
                            RefreshItem();

                            if (item_amount <= 0)
                                discardItem();

                            break;
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
        }

        /*Time.timeScale = (1);*/
        CharacterController.isOpen = false;
    }

    // remove the item from the inventory
    public void discardItem()
    {
        if (MyInventory == null || MyInventory.items == null)
        {
            Debug.LogWarning("Inventory or items list is not initialized.");
            return;
        }

        int discardIndex = MyInventory.items.FindIndex(item => item != null && item.itemName == item_name); // find the index of the item, neglect the null item

        // if the item will be discarded is the current use projectile, set the current use projectile to null
        if (MyInventory.items[discardIndex].itemName == "Fire Projectile")
        {
            if (characterController.projectilePrefab_currentUse == projectilePrefab_fire)
            {
                characterController.projectilePrefab_currentUse = null;
            }
        }
        else if (MyInventory.items[discardIndex].itemName == "Green Fire Projectile")
        {
            if (characterController.projectilePrefab_currentUse == projectilePrefab_greenFire)
            {
                characterController.projectilePrefab_currentUse = null;
            }
        }

        // discard the item
        if (discardIndex >= 0)
        {
            var itemToDiscard = MyInventory.items[discardIndex];

            if (itemToDiscard != null)
            {
                itemToDiscard.isPickedUp = false;
                itemToDiscard.item_amount = 1;

                MyInventory.items[discardIndex] = null;

                RefreshItem();
            }
        }
        else
        {
            Debug.LogWarning("Item not found in inventory.");
        }
    }

    /*public static void CreateNewItem(item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.item_icon;
        newItem.slotNum.text = item.item_amount.ToString();
    }*/

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        instance.slots.Clear();

        for (int i = 0; i < instance.MyInventory.items.Count; i++)
        {
            instance.slots.Add(Instantiate(instance.emptySlot));

            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotID = i;

            if (instance.MyInventory.items[i] != null)
            {
                instance.slots[i].GetComponent<Slot>().SetUpSlot(instance.MyInventory.items[i]);
            }
            else
            {
                instance.slots[i].GetComponent<Slot>().ClearSlot(); // for empty slots, clear the slot
            }
        }
    }

}
