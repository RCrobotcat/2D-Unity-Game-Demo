using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance; // Singleton

    public characterController CharacterController;

    public Inventory MyInventory;
    public GameObject slotGrid;
    /*public Slot slotPrefab;*/
    public GameObject emptySlot;
    public Text item_description;

    string item_name;
    bool item_equitable;

    public List<GameObject> slots = new List<GameObject>();

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

    public static void UpdateItemInfo(string itemDescription, string itemName, bool itemEquitable)
    {
        instance.item_description.text = itemDescription;

        instance.item_name = itemName;
        instance.item_equitable = itemEquitable;
    }

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

        Time.timeScale = (1);
        CharacterController.isOpen = false;
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
            if (instance.slotGrid.transform.GetChild(i).childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        for (int i = 0; i < instance.MyInventory.items.Count; i++)
        {
            /*CreateNewItem(instance.MyInventory.items[i]);*/

            instance.slots.Add(Instantiate(instance.emptySlot));

            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetUpSlot(instance.MyInventory.items[i]);
        }
    }
}
