using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        DraggableItem draggable = droppedItem.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.parentAfterDrag = this.transform; // Set the new parent to this slot
        }
    }


}
