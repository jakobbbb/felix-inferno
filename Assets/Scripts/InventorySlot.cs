using UnityEngine;
using UnityEngine.EventSystems;
using BugGame.Inventory; // Ensure this using is present

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedGO = eventData.pointerDrag;
        if (droppedGO == null) return;

        DraggableItem dropped = droppedGO.GetComponent<DraggableItem>();
        if (dropped == null) return;

        // Find existing draggable in this slot (exclude the one being dragged)
        DraggableItem existing = null;
        foreach (Transform child in transform)
        {
            if (child.gameObject == droppedGO) continue;
            var d = child.GetComponent<DraggableItem>();
            if (d != null)
            {
                existing = d;
                break;
            }
        }

        if (existing != null)
        {
            // Try to merge dropped.item and existing.item
            string a = dropped.item.displayName;
            string b = existing.item.displayName;
            Item result;
            if (InventoryManager.Instance != null && InventoryManager.Instance.TryGetMergeResult(a, b, out result))
            {
                Debug.Log($"[InventorySlot] Merge succeeded: {a} + {b} -> {result.displayName}");

                // Update inventory data
                InventoryManager.TakeItem(a);
                InventoryManager.TakeItem(b);
                InventoryManager.GiveItem(result.displayName);
                InventoryManager.GiveItem("Edding"); // Add the merged item to inventory

                // Remove the two UI items
                Destroy(existing.gameObject);
                Destroy(droppedGO);

                // Refresh UI to show merged result
                if (InventoryUI.Instance != null)
                {
                    InventoryUI.Instance.Refresh();
                    // Optionally keep inventory open:
                    InventoryUI.Instance.Show();
                }
                return;
            }
            else
            {
                Debug.Log("[InventorySlot] Items cannot be merged. Returning dragged item to original parent.");
                // Do not set parentAfterDrag — letting dragged item return to its original parent
                return;
            }
        }
        else
        {
            // Empty slot — place the dropped item here
            dropped.parentAfterDrag = this.transform;
        }
    }
}
