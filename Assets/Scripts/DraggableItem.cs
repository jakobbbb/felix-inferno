using BugGame.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform parentAfterDrag;
    public Image itemImage; // Reference to the Image component for visual feedback
    public BugGame.Inventory.Item item;

    private void Awake()
    {
        if (itemImage == null)
            itemImage = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        Transform root = transform.parent.parent;
        transform.SetParent(root); // Move to top level to avoid being clipped by parent
        transform.SetAsLastSibling(); // Ensure it's on top of other UI elements
        itemImage.raycastTarget = false; // Disable raycast to allow drop detection

        CursorUI.Instance.isDragging = true; // Set dragging state in CursorUI
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag); // Return to original parent
        itemImage.raycastTarget = true; // Re-enable raycast

        CursorUI.Instance.isDragging = false; // Clear dragging state in CursorUI
    }


}
