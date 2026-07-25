using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public ShopManager shop;
    public String slot;

    public GameObject currentItem;

    [Tooltip("Drag the ghost/silhouette placeholder GameObject for this slot here (the one still showing behind the dropped part).")]
    public GameObject placeholder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shop = GameObject.Find("ShopManager").GetComponent<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDropItem item = eventData.pointerDrag.GetComponent<DragDropItem>();
            if (item == null || item.part == null)
            {
                return;
            }

            // Only accept a drop if the part actually belongs in this slot
            bool validType =
                ((slot == "leftarm" || slot == "rightarm") && item.part.bodypart == "arm") ||
                ((slot == "leftleg" || slot == "rightleg") && item.part.bodypart == "leg") ||
                (slot == "head" && item.part.bodypart == "head");

            if (!validType)
            {
                return;
            }

            if (currentItem != null)
            {
                Destroy(currentItem);
            }

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            item.inSlot = true;
            item.slotType = slot;
            currentItem = eventData.pointerDrag;

            // Hide the placeholder art now that a real part is filling this slot
            if (placeholder != null)
            {
                placeholder.SetActive(false);
            }
        }
    }

    // Call this if the slot's part is ever removed, so the placeholder reappears
    public void ClearItem()
    {
        currentItem = null;
        if (placeholder != null)
        {
            placeholder.SetActive(true);
        }
    }
}