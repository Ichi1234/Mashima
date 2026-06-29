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

            int puzzleCurAmountItem = itemsCurAmount[item.itemData];

            Debug.Log("Interact " + itemInInventory);

            int remaining = item.requiredAmount - puzzleCurAmountItem;

            int amountToDeposit = Mathf.Min(itemInInventory, remaining);

            ItemManager.Instance.RemoveItem(item.itemData, amountToDeposit);
            itemsCurAmount[item.itemData] += amountToDeposit;

            if (itemsCurAmount[item.itemData] == item.requiredAmount)
            {
                item.requirementMet = true;
            }
        }

        isPuzzleCompleted = CheckedIsCompleted();
    }
}
