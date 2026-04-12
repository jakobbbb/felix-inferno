csharp Assets\Scripts\MergeRecipe.cs
using UnityEngine;
using BugGame.Inventory;

[CreateAssetMenu(fileName = "MergeRecipe", menuName = "Inventory/Merge Recipe", order = 0)]
public class MergeRecipe : ScriptableObject
{
    public Item itemA;
    public Item itemB;
    public Item result;

    public bool Matches(Item a, Item b)
    {
        if (a == null || b == null) return false;
        return (a == itemA && b == itemB) || (a == itemB && b == itemA);
    }
}