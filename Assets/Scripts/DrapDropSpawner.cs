using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrapDropSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject itemPrefab;

    public Part part;

    public Canvas canvas;

    public TextMeshProUGUI count;
    public UnityEngine.UI.Image icon;
    

    void Start()
    {
        if (canvas == null)
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            icon.sprite = part.icon;
    }

    // Update is called once per frame
    void Update()
    {
        count.text = part.numCollected.ToString();
        if (part.numCollected == 0 ){
            icon.color = new Color(1, 1, 1, 0.2f);
        } else
        {
            icon.color = new Color(1, 1, 1, 1f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
{
    //Debug.Log("Pointer down on spawner");
}

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Spawn");
        if (part.numCollected == 0 ){return;}
        GameObject clone = Instantiate(itemPrefab, canvas.transform);
        DragDropItem dragItem = clone.GetComponent<DragDropItem>();
        dragItem.part = part;

        // Position the clone at the spawner's position first
        RectTransform cloneRect = clone.GetComponent<RectTransform>();
        RectTransform spawnerRect = GetComponent<RectTransform>();
        cloneRect.position = spawnerRect.position;

        // Redirect the current drag to the clone
        eventData.pointerDrag = clone;

        // Manually fire OnBeginDrag on the clone since it just entered the drag
        ExecuteEvents.Execute(clone, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Intentionally empty — control is handed off to the clone in OnBeginDrag
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Intentionally empty — same reasoning
    }
}
