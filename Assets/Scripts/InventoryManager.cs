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


        public bool TryGetMergeResult(string itemAName, string itemBName, out Item result)
        {
            result = null;
            Item itemA = s_Inventory.FindItemInInventory(itemAName);
            Item itemB = s_Inventory.FindItemInInventory(itemBName);
            if (itemA == null || itemB == null) {
                Debug.LogError($"One or both items not found in inventory: {itemAName}, {itemBName}");
                return false;
            }

            // For simplicity, let's say we have a hardcoded merge result for "Key" + "Lock" = "UnlockedDoor"
            if (itemAName == "" && itemBName == "")
            {
                result = findItemInAllItems("");
                return true;
            }



            return false;
        }

        public List<Item> GetItems() {
            return s_Inventory.GetItems();
        }



    //find the item in all items list, then add it to inventory
    Item findItemInAllItems(string itemname) {
            foreach (var i in AllItems) {
                if (i.name == itemname || i.displayName == itemname) {
                    return i;
                }
            }
            return null;
        }

    }


}  // BugGame.Inventory
