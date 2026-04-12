using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorUI : MonoBehaviour
{
    [SerializeField] private InputActionReference _pointerPositionAction;
    private RectTransform _cursorTransform;
    private Canvas _parentCanvas;
    private RectTransform _canvasRectTransform;
    private Camera _canvasCamera;

    public static CursorUI Instance { get; private set; }

    [Tooltip("List of cursor definitions. Name values should match: Hover, Selected, Dragging (case-insensitive)")]
    public GameObject HoverCursor;
    public GameObject SelectedCursor;
    public GameObject DraggingCursor;

    public string _currentCursorName = null;



    private void Awake()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //Instance = this;
        //DontDestroyOnLoad(gameObject);

        _cursorTransform = GetComponent<RectTransform>();
        _parentCanvas = GetComponentInParent<Canvas>();
        if (_parentCanvas != null)
        {
            _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();
            _canvasCamera = _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera;
        }

    }

    private void OnEnable()
    {
        
        _pointerPositionAction.action.performed += OnPointerPositionChanged;
    }

    private void OnDisable()
    {
        _pointerPositionAction.action.performed -= OnPointerPositionChanged;
    }

    void OnPointerPositionChanged(InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPos, _canvasCamera, out localPoint))
        {
            _cursorTransform.localPosition = localPoint;
        }
    }

    private void Update()
    {

        _currentCursorName = "Hover";

        if (Mouse.current.leftButton.isPressed)
        {
            _currentCursorName = "Selected";
        }

        SetCursor(_currentCursorName);
    }


    void SetCursor(string cursorName)
    {
        HoverCursor.SetActive(cursorName == "Hover");
        SelectedCursor.SetActive(cursorName == "Selected");
        DraggingCursor.SetActive(cursorName == "Dragging");
    }


}
