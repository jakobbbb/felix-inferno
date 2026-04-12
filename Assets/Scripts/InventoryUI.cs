using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Collections.Generic;
using BugGame.Inventory;

public class InventoryUI : MonoBehaviour {

    public static InventoryUI Instance { get; private set; }

    [Header("References")]
    [Tooltip("Parent transform where item buttons will be spawned (e.g., a Vertical/Grid Layout Group).")]
    [SerializeField] private Transform itemContainer;
    [Tooltip("Prefab for each inventory item button.")]
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private GameObject inventoryCanvas;

    [Header("Settings")]
    [Tooltip("Toggle the inventory UI with this key.")]
    [SerializeField] private bool toggleWithKey = true;
    [SerializeField] private Key toggleKey = Key.I;

    /// <summary>
    /// Fired when an inventory item button is clicked. Passes the clicked Item.
    /// </summary>
    public event Action<Item> OnItemClicked;

    private List<GameObject> spawnedButtons = new List<GameObject>();

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false);
    }

    private void Update() {
        if (toggleWithKey && Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame) {
            if (inventoryCanvas.activeSelf) {
                Hide();
            } else {
                Show();
            }
        }
    }

    /// <summary>
    /// Shows the inventory UI and rebuilds the item list.
    /// </summary>
    public void Show() {
        inventoryCanvas.SetActive(true);
        Refresh();
    }

    /// <summary>
    /// Hides the inventory UI.
    /// </summary>
    public void Hide() {
        inventoryCanvas.SetActive(false);
        ClearButtons();
    }

    /// <summary>
    /// Rebuilds all item buttons from the current inventory state.
    /// </summary>
    public void Refresh() {
        ClearButtons();

        List<Item> items = InventoryManager.Instance.GetItems();

        foreach (var item in items) 
        {
            //find the first childless transform in the container to spawn the item under it
            Transform container = FindChildless(itemContainer);

            //if no childless transform is found, log a warning and skip this item
            if (container == null )
            {
                Debug.LogWarning("[InventoryUI] No available container found for item: " + item.displayName);
                continue;
            }

            //GameObject draggableObject = Instantiate(itemButtonPrefab, container);
            GameObject draggableObject = new GameObject(item.displayName);
            draggableObject.transform.SetParent(container, false);
            // Set icon
            Image iconImage = draggableObject.AddComponent<Image>();
            if (iconImage != null && item.icon != null) {
                iconImage.sprite = item.icon;
            }
            draggableObject.AddComponent<DraggableItem>();

            //attach click event
            Button button = draggableObject.AddComponent<Button>();
            if (button != null) {
                Item capturedItem = item; // capture for closure
                button.onClick.AddListener(() => {
                    Debug.Log($"[InventoryUI] Clicked item: {capturedItem.displayName}");
                    OnItemClicked?.Invoke(capturedItem);
                });
            }





        }


        //foreach (var item in items) {
        //    GameObject buttonObj = Instantiate(itemButtonPrefab, itemContainer);
        //    spawnedButtons.Add(buttonObj);

        //    // Set icon
        //    Image iconImage = buttonObj.transform.Find("Icon")?.GetComponent<Image>();
        //    if (iconImage != null && item.icon != null) {
        //        iconImage.sprite = item.icon;
        //    }

        //    // Set name
        //    TMP_Text nameText = buttonObj.transform.Find("ItemName")?.GetComponent<TMP_Text>();
        //    if (nameText != null) {
        //        nameText.text = item.displayName;
        //    }

        //    // Attach click event
        //    Button button = buttonObj.GetComponent<Button>();
        //    if (button != null) {
        //        Item capturedItem = item; // capture for closure
        //        button.onClick.AddListener(() => {
        //            Debug.Log($"[InventoryUI] Clicked item: {capturedItem.displayName}");
        //            OnItemClicked?.Invoke(capturedItem);
        //        });
        //    }
        //}
    }

    Transform FindChildless(Transform parent) { 



        for (int i = 0; i < parent.childCount; i++) {
            
            if (parent.GetChild(i).childCount == 0) {
                return parent.GetChild(i);
            }
        }

        return null;
    }


    private void ClearButtons() {
        foreach (Transform child in itemContainer)
        {
            if (child.childCount > 0)
            {
                Destroy(child.GetChild(0).gameObject);
            }
        }
    }
}