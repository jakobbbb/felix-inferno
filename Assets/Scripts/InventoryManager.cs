using Yarn.Unity;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace BugGame.Inventory {

public class InventoryManager : MonoBehaviour {

    public List<Item> AllItems;

    private static Inventory s_Inventory = new Inventory();
    public static InventoryManager Instance = null;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        AllItems = (Resources.LoadAll<Item>("Items") as Item[]).ToList();
        Debug.Log($"Got {AllItems.Count} items!");
    }

    [YarnFunction("has_item")]
    public static bool HasItem(string itemname) {
        return s_Inventory.HasItem(itemname);
    }

    [YarnCommand("give_item")]
    public static void GiveItem(string itemname) {
        Debug.Log($"give_item {itemname}");

        Item item = null;

        foreach (var i in Instance.AllItems) {
            if (i.name == itemname || i.displayName == itemname) {
                item = i;
                break;
            }
        }

        if (item != null) {
            s_Inventory.AddItem(item);
        } else {
            Debug.LogError($"Item {itemname} not found :(");
        }
    }

    [YarnCommand("take_item")]
    public static void TakeItem(string itemname) {
        var item = s_Inventory.FindItemInInventory(itemname);
        s_Inventory.RemoveItem(item);
    }

    public List<Item> GetItems() {
        return s_Inventory.GetItems();
    }

}

}  // BugGame.Inventory
