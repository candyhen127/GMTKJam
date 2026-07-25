using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrapDropSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject itemPrefab;

    public Part part;

    public Canvas canvas;

    public TextMeshProUGUI count;
    

    void Start()
    {
        if (canvas == null)
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        count.text = part.numCollected.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
{
    //Debug.Log("Pointer down on spawner");
}

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Spawn");
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
