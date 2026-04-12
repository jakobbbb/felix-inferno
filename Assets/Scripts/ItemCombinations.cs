using UnityEngine;

public class ItemCombinations : MonoBehaviour {

    private string ItemCombination(string item_a, string item_b) {
        if (item_a == "piranhaplant" && item_b == "tofu") {

        }

        return null;
    }

    public string CombineItems(string item_a, string item_b) {
        var dia = ItemCombination(item_a, item_b);
        if (dia != null) {
            return dia;
        }
        return ItemCombination(item_b, item_a);
    }
}
