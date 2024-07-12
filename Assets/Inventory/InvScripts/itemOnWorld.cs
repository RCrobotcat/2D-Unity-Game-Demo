using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemOnWorld : MonoBehaviour
{
    public item thisItem;
    public Inventory playerInventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        if (!playerInventory.items.Contains(thisItem))
        {
            /*playerInventory.items.Add(thisItem);*/
            /*InventoryManager.CreateNewItem(thisItem);*/

            for (int i = 0; i < playerInventory.items.Count; i++)
            {
                if (playerInventory.items[i] == null)
                {
                    playerInventory.items[i] = thisItem;
                    break;
                }
            }

        }
        else
        {
            thisItem.item_amount += 1;
        }

        InventoryManager.RefreshItem(); // Refresh the item in the inventory
    }
}
