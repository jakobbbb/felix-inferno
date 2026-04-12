using UnityEngine;

namespace BugGame.Inventory {

[CreateAssetMenu(fileName = "Item", menuName = "Inventory Item", order = 0)]
public class Item : ScriptableObject {
    public string displayName;
        public string dialogueReference;
    public Sprite icon = null;

    public int GetId() {
        return name.GetHashCode();
    }
}

}  // BugGame.Inventory
