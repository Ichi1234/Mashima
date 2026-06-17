using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    public ItemData ItemData => itemData;

    public void Interact()
    {
        ItemManager.Instance.StoreItem(this);

        Destroy(gameObject);
    }
}
