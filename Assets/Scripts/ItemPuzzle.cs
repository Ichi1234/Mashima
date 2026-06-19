using Newtonsoft.Json.Bson;
using UnityEngine;

public class ItemPuzzle : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private int requiredAmount = 1;
    [SerializeField] private SpriteRenderer sr;

    private int curAmount = 0;

    private bool isPuzzleCompleted = false;

    private void Update()
    {
        if (isPuzzleCompleted)
        {
            sr.color = Color.red;
        }
    }

    public void Interact()
    {
        if (isPuzzleCompleted) return;

        int curAmountOfItem = ItemManager.Instance.GetItem(requiredItem);

        Debug.Log("Interact " + curAmountOfItem);

        if (curAmountOfItem <= requiredAmount)
        {
            ItemManager.Instance.RemoveItem(requiredItem, curAmountOfItem);
            curAmount += curAmountOfItem;
        }

        else if (curAmountOfItem > requiredAmount)
        {
            ItemManager.Instance.RemoveItem(requiredItem, requiredAmount);
            curAmount += requiredAmount;
        }

        if (curAmount == requiredAmount)
        {
            isPuzzleCompleted = true;
        }

    }
}
