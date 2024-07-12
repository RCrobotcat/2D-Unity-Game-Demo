using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class moveBag : MonoBehaviour, IDragHandler
{
    public Canvas canvas;
    RectTransform currentRect;
    private void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentRect.anchoredPosition += eventData.delta; // move the bag, based on the mouse movement
        // delta: The change in the position of the cursor since the last frame.
    }
}
