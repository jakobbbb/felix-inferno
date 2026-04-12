using UnityEngine;

public class ItemCombinations : MonoBehaviour {

    private string ItemCombination(string item_a, string item_b) {
        if (item_a == "edding" && item_b == "gamejamposter") {
            return "edding_gamejamposter";
        }
      
        if (item_a == "edding" && item_b == "hamster")
        {
            return "edding_hamster";
        }

        if (item_a == "tofu" && item_b == "piranhaplant")
        {
            return "tofu_piranhaplant";
        }

        if (item_a == "vacuum" && item_b == "couch")
        {
            return "vacuum_couch";
        }

        if (item_a == "vacuum" && item_b == "hamster")
        {
            return "vacuum_hamster";
        }

        if (item_a == "edding" && item_b == "fireextinguisher")
        {
            return "edding_fireextinguisher";
        }

        if (item_a == "frozenhamster" && item_b == "microwave")
        {
            return "frozenhamster_microwave";
        }

        if (item_a == "frozenhamster" && item_b == "aquarium")
        {
            return "frozenhamster_aquarium";
        }

        if (item_a == "hamster" && item_b == "aquarium")
        {
            return "hamster_aquarium";
        }

        if (item_a == "person3" && item_b == "bottle")
        {
            return "bottle_person3";
        }

        if (item_a == "person5" && item_b == "books")
        {
            return "books_person5";
        }

        if (item_a == "person3" && item_b == "bottle")
        {
            return "bottle_person3";
        }

        if (item_a == "key" && item_b == "door")
        {
            return "key_door";
        }

        if (item_a == "gamejamposter" && item_b == "wall")
        {
            return "gamejamposter_wall";
        }

        if (item_a == "dinotattoos" && item_b == "Felix")
        {
            return "dinotattoos_Felix";
        }

        if (item_a == "key" && item_b == "hamster")
        {
            return "animalcruelty";
        }

        if (item_a == "gamejamposter" && item_b == "hamster")
        {
            return "animalcruelty";
        }

        if (item_a == "fireextinguisher" && item_b == "hamster")
        {
            return "animalcruelty";
        }
        if (item_a == "scissors" && item_b == "hamster")
        {
            return "animalcruelty";
        }

        return "nointeraction";
    }

  
    public string CombineItems(string item_a, string item_b) {
        var dia = ItemCombination(item_a, item_b);
        if (dia != null) {
            return dia;
        }
        return ItemCombination(item_b, item_a);
    }
}
