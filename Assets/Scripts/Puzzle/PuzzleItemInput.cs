using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PuzzleItemInput : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleID puzzleID;
    [SerializeField] private MonoBehaviour puzzleReactorObject;
    [SerializeField] private List<PuzzleItemRequirement> requiredItems;

    [SerializeField] private Indicator indicator;

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

        indicator.SetShowable(false);
    }

    private void Update()
    {
        if (isPuzzleCompleted) return;

        CheckShowIndicator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.IsInVR) return;
        if (!other.CompareTag("Item")) return;

        Item droppedItem = other.GetComponent<Item>();
        if (droppedItem == null) return;

        foreach (PuzzleItemRequirement requireItem in requiredItems)
        {
            if (requireItem.itemData != droppedItem.ItemData) continue;
            if (requireItem.requirementMet) continue;

            DepositItem(requireItem, other.gameObject);
            break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item goneItem = other.GetComponent<Item>();
        if (goneItem == null) return;

        foreach (var item in requiredItems)
        {
            if (goneItem.ItemData != item.itemData) continue;

            itemsCurAmount[item.itemData] = Mathf.Max(0, itemsCurAmount[item.itemData] - 1);
            item.requirementMet = itemsCurAmount[item.itemData] >= item.requiredAmount;
            break;
        }
    }

    private void CheckShowIndicator()
    {
        bool hasRelevantItem = false;

        foreach (PuzzleItemRequirement item in requiredItems)
        {
            if (item.requirementMet) continue;
            if (ItemManager.Instance.GetItem(item.itemData) <= 0) continue;
            hasRelevantItem = true;
            break;
        }

        if (hasRelevantItem != indicator.IsShowable)
        {
            indicator.SetShowable(hasRelevantItem);
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

    private void DepositItem(PuzzleItemRequirement item, GameObject vrItem = null)
    {
        itemsCurAmount[item.itemData] += 1;
        Debug.Log($"Placed 1x {item.itemData.name} ({itemsCurAmount[item.itemData]}/{item.requiredAmount})");

        if (itemsCurAmount[item.itemData] >= item.requiredAmount)
        {
            item.requirementMet = true;
        }

        if (GameManager.Instance.IsInVR)
        {
            puzzleReactor?.OnItemDeposited(vrItem);
        }
        else
        {
            puzzleReactor?.OnItemDeposited(item.itemData.itemPrefab);
        }

        isPuzzleCompleted = CheckedIsCompleted();
        
        if (isPuzzleCompleted)
        {
            PuzzleManager.Instance.SetPuzzleState(puzzleID, PuzzleState.Completed);
            puzzleReactor?.OnPuzzleCompleted();
            indicator.SetShowable(false);
        }
    }
}