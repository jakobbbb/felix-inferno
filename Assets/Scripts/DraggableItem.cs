using BugGame.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform parentAfterDrag;
    public Image itemImage; // Reference to the Image component for visual feedback
    public BugGame.Inventory.Item item;

    public RectTransform rectTransform; // root rect to check against

    [Header("Exit / Enter Events")]
    [Tooltip("Invoked when the dragged item leaves the rectTransform bounds.")]
    public UnityEvent onExitRoot;
    [Tooltip("Invoked when the dragged item re-enters the rectTransform bounds.")]
    public UnityEvent onEnterRoot;

    // internal state
    public bool _isDragging;
    public bool _isInsideRoot;
    private bool windowClosed = false; // Track if the inventory window has been closed during dragging

    private Canvas _rootCanvas;

    private void Awake()
    {
        if (itemImage == null)
            itemImage = GetComponent<Image>();

        // If rectTransform wasn't assigned in inspector, try to use parent.parent's RectTransform (existing behavior),
        // otherwise try to find the parent Canvas rect.
        if (rectTransform == null)
        {
            if (transform.parent != null && transform.parent.parent != null)
            {
                rectTransform = transform.parent.parent.GetComponent<RectTransform>();
            }
        }

        // Also find the containing canvas for camera reference
        _rootCanvas = GetComponentInParent<Canvas>();
        if (_rootCanvas == null && rectTransform != null)
            _rootCanvas = rectTransform.GetComponentInParent<Canvas>();

        // initialize inside/outside state based on current mouse position
        Vector2 screenPos = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        _isInsideRoot = IsScreenPointInsideRect(rectTransform, screenPos, CanvasCamera());
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;

        parentAfterDrag = transform.parent;
        Transform root = transform.parent != null ? transform.parent.parent : transform;
        transform.SetParent(root); // Move to top level to avoid being clipped by parent
        transform.SetAsLastSibling(); // Ensure it's on top of other UI elements
        if (itemImage != null) itemImage.raycastTarget = false; // Disable raycast to allow drop detection

        CursorUI.Instance.isDragging = true; // Set dragging state in CursorUI
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move with mouse
        Vector2 pos = Mouse.current != null ? Mouse.current.position.ReadValue() : eventData.position;
        transform.position = pos;

        // Only check bounds while dragging and if rectTransform is available
        if (rectTransform == null) return;

        bool nowInside = IsScreenPointInsideRect(rectTransform, pos, CanvasCamera());

        // detect transitions
        if (_isInsideRoot && !nowInside)
        {
            _isInsideRoot = false;
            //onExitRoot?.Invoke();

            parentAfterDrag = parentAfterDrag.parent.parent.parent; // Store current parent before moving
            transform.SetParent(parentAfterDrag); // Move back to original parent when exiting

            Debug.Log("Exited root bounds, hiding parent");
            rectTransform.transform.parent.gameObject.SetActive(false); // Hide the parent of the rectTransform when exiting
            windowClosed = true; // Mark that we've closed the window during this drag
        }
        else if (!_isInsideRoot && nowInside)
        {
            _isInsideRoot = true;
            onEnterRoot?.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag); // Return to original parent
        if (itemImage != null) itemImage.raycastTarget = true; // Re-enable raycast

        CursorUI.Instance.isDragging = false; // Clear dragging state in CursorUI

        _isDragging = false;
        

        //dropped outside the canvas, delete put it on an object and if no combo then delete the object
        if (windowClosed)
        {
            Debug.Log("Dropped inside root bounds, showing parent");
            //rectTransform.transform.parent.gameObject.SetActive(true); // Ensure parent is visible when dropped inside

            LayerMask layerMask = LayerMask.GetMask("Interactable");
            var hit = RayUtils.CastMouseRayToWorld(layerMask);
            if (hit.HasValue)
            {
                var interactable = hit.Value.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    Debug.Log("Interacted with object: " + interactable.name);
                    string item3d = interactable.name;
                    string myName = item.name;   
                    string dialgoue = ItemCombinations.Instance.CombineItems(myName, item3d);
                    Debug.Log("Combination result: " + dialgoue);

                    if(!string.IsNullOrEmpty(dialgoue))
                    {
                        DialogueManager.Instance.StartDialogue(dialgoue);
                        Debug.Log("Successful combination! Triggering dialogue and destroying item.");

                    }

                    
                }
            }
            else
            {
                Debug.Log("No interactable object hit. Item will be destroyed.");
            }

            Destroy(gameObject); // Destroy the dragged item after interaction
        }

    }

    private Camera CanvasCamera()
    {
        if (_rootCanvas == null) return Camera.main;
        if (_rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) return null;
        if (_rootCanvas.worldCamera != null) return _rootCanvas.worldCamera;
        return Camera.main;
    }

    private static bool IsScreenPointInsideRect(RectTransform rect, Vector2 screenPoint, Camera cam)
    {
        if (rect == null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, cam);
    }
}
