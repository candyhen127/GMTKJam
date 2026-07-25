using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragDropItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    
    public RectTransform rectTransform;

    public CanvasGroup canvasGroup;

    public Canvas canvas;

    public Part part;

    public bool inSlot;

    public string slotType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!inSlot){
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        GetComponent<Image>().sprite = part.icon;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        //canvasGroup.blocksRaycasts = true;
        if (!inSlot || 
                (((slotType == "leftarm") || (slotType == "rightarm")) && part.bodypart != "arm") ||
                (((slotType == "leftleg") || (slotType == "rightleg")) && part.bodypart != "leg") ||
                ((slotType == "head") && part.bodypart != "head"))
        {
            Destroy(gameObject);
        } else
        {
            GameObject.Find("ShopManager").GetComponent<ShopManager>().setPart(slotType, part);
        }
        
    }
    



}
