using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;

    public Inventory myInventory;
    int currentItemID; // The ID of the current item

    public Slot slot_current; // The slot that the item is currently in

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        /*Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            // If the item is dropped on the itemImage, then swap the items
            if (eventData.pointerCurrentRaycast.gameObject.transform.name == "itemImage")
            {
                // Swap the items' ID 
                var temp = myInventory.items[currentItemID];
                myInventory.items[currentItemID] = myInventory.items[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                myInventory.items[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;

                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent); // set parent to "slot"
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position; // switch position to "slot"

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position; // switch position to original position
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent); // set parent to original parent

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                InventoryManager.RefreshItem();
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.transform.name == "slot(Clone)")
            {
                myInventory.items[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myInventory.items[currentItemID]; // set the item to the new slot
                if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID != currentItemID)
                    myInventory.items[currentItemID] = null; // remove the item from the old slot   

                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                InventoryManager.RefreshItem();
                return;
            }
        }

        // other cases, set the item back to the original position
        transform.SetParent(originalParent);
        transform.position = originalParent.position;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
