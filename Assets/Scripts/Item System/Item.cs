using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    [SerializeField] private bool isInteractable = true;

    public ItemData ItemData => itemData;


    public void Interact()
    {
        if (!isInteractable) return;

        ItemManager.Instance.StoreItem(itemData);

        Destroy(gameObject);
    }

    public void EnableInteract() => isInteractable = true;
    public void DisableInteract() => isInteractable = false;
}
