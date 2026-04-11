using UnityEngine;
using System.Collections.Generic;

namespace BugGame.Inventory {

[System.Serializable]
public class Inventory {
    private List<Item> m_Items = new List<Item>();

    public void AddItem(Item i) {
        m_Items.Add(i);
        Debug.Log($"Added item {i.displayName}");
    }

    public void RemoveItem(Item i) {
        if (m_Items.Contains(i)) {
            m_Items.Remove(i);
            Debug.Log($"Removed item {i.displayName}");
        } else {
            Debug.Log($"Did not have item {i.displayName}. Not removing.");
        }
    }

    public Item FindItemInInventory(string itemname) {
        foreach (var i in m_Items) {
            if (i.name == itemname || i.displayName == itemname) {
                return i;
            }
        }
        return null;
    }

    public bool HasItem(string itemname) {
        return FindItemInInventory(itemname) != null;
    }

    public List<Item> GetItems() {
        return m_Items;
    }

}
}  // BugGame.Inventory
