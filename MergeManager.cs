using System.Collections.Generic;
using UnityEngine;
using BugGame.Inventory;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance { get; private set; }

  
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public bool TryGetMergeResult(Item a, Item b, out Item result)
    {
        result = null;
        if (a == null || b == null) return false;

        foreach (var r in recipes)
        {
            if (r != null && r.Matches(a, b))
            {
                result = r.result;
                return result != null;
            }
        }

        return false;
    }
}