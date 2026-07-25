using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public ShopManager shop;
    public String slot;

    public GameObject currentItem;

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
            if (currentItem != null)
            {
                Destroy(currentItem);
            }
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            DragDropItem item =eventData.pointerDrag.GetComponent<DragDropItem>(); 
            Part part = item.part;
            item.inSlot = true;
            item.slotType = slot;
            currentItem = eventData.pointerDrag;
            
        }
    }
}
