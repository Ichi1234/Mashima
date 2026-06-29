using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemInput : MonoBehaviour, IInteractable
{
    [SerializeField] private List<PuzzleItemRequirement> requiredItems;
    private Dictionary<ItemData, int> itemsCurAmount;
    private bool isPuzzleCompleted = false;

    private void Awake()
    {
        itemsCurAmount = new Dictionary<ItemData, int>();
        foreach (var item in requiredItems)
        {
            itemsCurAmount.Add(item.itemData, 0);
        }
    }

    private bool CheckedIsCompleted()
    {
        foreach (var item in requiredItems)
        {
            if (!item.requirementMet) return false;
        }
        return true;
    }

    public void Interact()
    {
        if (isPuzzleCompleted) return;

        foreach (var item in requiredItems)
        {
            if (item.requirementMet) continue;

            int itemInInventory = ItemManager.Instance.GetItem(item.itemData);

            if (itemInInventory <= 0) continue;

            ItemManager.Instance.RemoveItem(item.itemData, 1);
            itemsCurAmount[item.itemData] += 1;

            Debug.Log($"Placed 1x {item.itemData.name} ({itemsCurAmount[item.itemData]}/{item.requiredAmount})");

            if (itemsCurAmount[item.itemData] >= item.requiredAmount)
            {
                item.requirementMet = true;
            }

            break;
        }

        isPuzzleCompleted = CheckedIsCompleted();
        if (isPuzzleCompleted) Debug.Log("Owatta!");
    }
}