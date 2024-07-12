using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class itemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If the item is dropped on the itemImage, then swap the items
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name == "itemImage")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent); // set parent to "slot"
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position; // switch position to "slot"

            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position; // switch position to original position
            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent); // set parent to original parent

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }

        transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
        transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
