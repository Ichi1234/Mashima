using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    private Dictionary<string, int> backpack;

    private void Awake()
    {
        backpack = new Dictionary<string, int>();
    }

    public int GetItem(ItemData item)
    {
        if (!backpack.ContainsKey(item.name))
        {
            return 0;
        }
        return backpack[item.name];
    }
    public Dictionary<string, int> GetAllItems() => backpack;

    public void StoreItem(ItemData newItem)
    {
        if (!backpack.ContainsKey(newItem.name))
        {
            backpack.Add(newItem.name, 1);
        }

        else
        {
            backpack[newItem.name] += 1;
        }

        Debug.Log("Amount of " + newItem.name + " are " + backpack[newItem.name]);

    }
    public void RemoveItem(ItemData removeItem, int amount = 1)
    {
        if (!backpack.ContainsKey(removeItem.name))
        {
            return;
        }

        backpack[removeItem.name] -= amount;

        Debug.Log("OH Nyo we lost some item " + backpack[removeItem.name]);
    }
}

