using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    private Dictionary<string, int> backpack;

    private void Awake()
    {
        backpack = new Dictionary<string, int>();
    }

    public int GetItem(string itemName) => backpack[itemName];

    public Dictionary<string, int> GetAllItems() => backpack;

    public void StoreItem(Item newItem)
    {
        if (!backpack.ContainsKey(newItem.name))
        {
            backpack.Add(newItem.name, 1);
        }

        else
        {
            backpack[newItem.name] += 1;
        }
    }
    public void RemoveItem(Item removeItem)
    {
        if (!backpack.ContainsKey(removeItem.name))
        {
            return;
        }

        backpack[removeItem.name] -= 1;
    }
}

