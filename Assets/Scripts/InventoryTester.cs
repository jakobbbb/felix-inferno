using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BugGame.Inventory;

public class InventoryTester : MonoBehaviour {

    [Header("Controls")]
    [Tooltip("Press this key to add all items from InventoryManager.AllItems to the inventory.")]
    [SerializeField] private Key addAllKey = Key.F1;
    [Tooltip("Press this key to add the next item one at a time.")]
    [SerializeField] private Key addOneKey = Key.F2;
    [Tooltip("Press this key to remove all items from the inventory.")]
    [SerializeField] private Key clearKey = Key.F3;

    private int currentIndex = 0;

    private void Update() {
        if (Keyboard.current == null) {
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
    /// Adds every item from InventoryManager.AllItems into the player's inventory.
    /// </summary>
    private void AddAllItems() {
        List<Item> allItems = InventoryManager.Instance.AllItems;

        if (allItems == null || allItems.Count == 0) {
            Debug.LogWarning("[InventoryTester] No items found in InventoryManager.AllItems.");
            return;
        }

        foreach (var item in allItems) {
            Debug.Log($"[InventoryTester] Adding: {item.displayName}");
            InventoryManager.GiveItem(item.displayName);
        }

        currentIndex = allItems.Count;
        Debug.Log($"[InventoryTester] Added all {allItems.Count} items.");
        RefreshUI();
    }

    /// <summary>
    /// Adds the next item from InventoryManager.AllItems one at a time.
    /// </summary>
    private void AddNextItem() {
        List<Item> allItems = InventoryManager.Instance.AllItems;

        if (allItems == null || allItems.Count == 0) {
            Debug.LogWarning("[InventoryTester] No items found in InventoryManager.AllItems.");
            return;
        }

        if (currentIndex >= allItems.Count) {
            Debug.Log("[InventoryTester] All items already added. Press Clear to reset.");
            return;
        }

        var item = allItems[currentIndex];
        InventoryManager.GiveItem(item.displayName);
        Debug.Log($"[InventoryTester] Added ({currentIndex + 1}/{allItems.Count}): {item.displayName}");
        currentIndex++;

        RefreshUI();
    }

    /// <summary>
    /// Removes all items currently in the player's inventory.
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