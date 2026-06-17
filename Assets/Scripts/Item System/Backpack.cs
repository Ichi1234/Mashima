using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    private Dictionary<string, int> backpack;

    private void Awake()
    {
        backpack = new Dictionary<string, int>();
    }

    public int GetItem(Item item) => backpack[item.ItemData.name];

    public Dictionary<string, int> GetAllItems() => backpack;

    public void StoreItem(Item newItem)
    {
        if (!backpack.ContainsKey(newItem.ItemData.name))
        {
            backpack.Add(newItem.ItemData.name, 1);
        }

        else
        {
            backpack[newItem.ItemData.name] += 1;
        }
    }
    public void RemoveItem(Item removeItem)
    {
        if (!backpack.ContainsKey(removeItem.ItemData.name))
        {
            return;
        }

        backpack[removeItem.ItemData.name] -= 1;
    }
}

