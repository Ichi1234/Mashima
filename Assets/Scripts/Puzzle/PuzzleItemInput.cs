using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemInput : MonoBehaviour, IInteractable
{
    [SerializeField] private MonoBehaviour puzzleReactorObject;
    [SerializeField] private List<PuzzleItemRequirement> requiredItems;

    private IPuzzleReactable puzzleReactor;
    private Dictionary<ItemData, int> itemsCurAmount;
    private bool isPuzzleCompleted = false;

    private void Awake()
    {
        itemsCurAmount = new Dictionary<ItemData, int>();
        puzzleReactor = puzzleReactorObject as IPuzzleReactable;
        foreach (PuzzleItemRequirement item in requiredItems)
        {
            itemsCurAmount.Add(item.itemData, 0);
        }
    }

    private bool CheckedIsCompleted()
    {
        foreach (PuzzleItemRequirement item in requiredItems)
        {
            if (!item.requirementMet) return false;
        }
        return true;
    }

    public void Interact()
    {
        if (isPuzzleCompleted) return;

        foreach (PuzzleItemRequirement item in requiredItems)
        {
            if (item.requirementMet) continue;

            int itemInInventory = ItemManager.Instance.GetItem(item.itemData);
            if (itemInInventory <= 0) continue;

            ItemManager.Instance.RemoveItem(item.itemData, Mathf.Min(item.requiredAmount, 1));
            DepositItem(item);
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurPlayerMode != PlayerMode.VR) return;
        if (!other.CompareTag("Item")) return;

        Item droppedItem = other.GetComponent<Item>();
        if (droppedItem == null) return;

        foreach (PuzzleItemRequirement requireItem in requiredItems)
        {
            if (requireItem.itemData != droppedItem.ItemData) continue;
            if (requireItem.requirementMet) continue;

            DepositItem(requireItem);
            break;
        }
    }

    private void DepositItem(PuzzleItemRequirement item)
    {
        itemsCurAmount[item.itemData] += 1;
        Debug.Log($"Placed 1x {item.itemData.name} ({itemsCurAmount[item.itemData]}/{item.requiredAmount})");

        if (itemsCurAmount[item.itemData] >= item.requiredAmount)
        {
            item.requirementMet = true;
        }

        puzzleReactor?.OnItemDeposited(item.itemData.itemPrefab);

        isPuzzleCompleted = CheckedIsCompleted();
        if (isPuzzleCompleted) puzzleReactor?.OnPuzzleCompleted();
        if (isPuzzleCompleted) Debug.Log("Owatta!!!!");
    }
}