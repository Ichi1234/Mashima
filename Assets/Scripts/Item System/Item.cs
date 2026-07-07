using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    [SerializeField] private bool isInteractable = true;

    [SerializeField] private Indicator indicator;

    public ItemData ItemData => itemData;


    public void Interact()
    {
        if (!isInteractable) return;

        ItemManager.Instance.StoreItem(itemData);

        transform.position = new Vector3(999, 999, 999);

        Destroy(gameObject, 0.8f);
    }

    public void EnableInteract()
    {
        isInteractable = true;

        indicator.SetShowable(isInteractable);
    }

    public void DisableInteract()
    {
        isInteractable = false;
        indicator.SetShowable(isInteractable);

    }
}
