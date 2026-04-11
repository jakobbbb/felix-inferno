using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BugGame.Inventory;

public class InventoryTester : MonoBehaviour {

    [Header("Test Items")]
    [Tooltip("Drag Item ScriptableObjects here to pre-populate the inventory for testing.")]
    [SerializeField] private List<Item> testItems = new List<Item>();

    [Header("Controls")]
    [Tooltip("Press this key to add all test items to the inventory.")]
    [SerializeField] private Key addAllKey = Key.F1;
    [Tooltip("Press this key to add the next test item one at a time.")]
    [SerializeField] private Key addOneKey = Key.F2;
    [Tooltip("Press this key to clear all items from the inventory.")]
    [SerializeField] private Key clearKey = Key.F3;

    private int currentIndex = 0;

    private void Update() {
        if (Keyboard.current == null)
        {
            Debug.LogWarning("[InventoryTester] No keyboard detected. Inventory testing controls will not work.");
            return;
        }
            

        if (Keyboard.current[addAllKey].wasPressedThisFrame) {
            AddAllItems();
        }

        if (Keyboard.current[addOneKey].wasPressedThisFrame) {
            AddNextItem();
        }

        if (Keyboard.current[clearKey].wasPressedThisFrame) {
            ClearInventory();
        }
    }

    /// <summary>
    /// Adds all test items to the inventory at once.
    /// </summary>
    private void AddAllItems() {
        if (testItems.Count == 0) {
            Debug.LogWarning("[InventoryTester] No test items assigned.");
            return;
        }

        foreach (var item in testItems) {
            Debug.Log($"[InventoryTester] Adding: {item.displayName}");
            InventoryManager.GiveItem(item.displayName);
            Debug.Log($"[InventoryTester] Added: {item.displayName}");
        }

        currentIndex = testItems.Count;
        RefreshUI();
    }

    /// <summary>
    /// Adds the next item from the test list one at a time.
    /// </summary>
    private void AddNextItem() {
        if (testItems.Count == 0) {
            Debug.LogWarning("[InventoryTester] No test items assigned.");
            return;
        }

        if (currentIndex >= testItems.Count) {
            Debug.Log("[InventoryTester] All test items already added. Press Clear to reset.");
            return;
        }

        var item = testItems[currentIndex];
        InventoryManager.GiveItem(item.displayName);
        Debug.Log($"[InventoryTester] Added ({currentIndex + 1}/{testItems.Count}): {item.displayName}");
        currentIndex++;

        RefreshUI();
    }

    /// <summary>
    /// Removes all items from the inventory.
    /// </summary>
    private void ClearInventory() {
        var items = new List<Item>(InventoryManager.Instance.GetItems());

        foreach (var item in items) {
            InventoryManager.TakeItem(item.displayName);
        }

        currentIndex = 0;
        Debug.Log("[InventoryTester] Inventory cleared.");
        RefreshUI();
    }

    private void RefreshUI() {
        if (InventoryUI.Instance != null) {
            InventoryUI.Instance.Show();
        }
    }
}