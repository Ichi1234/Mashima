using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Backpack backpack;

    public static ItemManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StoreItem(ItemData item)
    {
        backpack.StoreItem(item);
    }

    public int GetItem(ItemData item) => backpack.GetItem(item);

    public void RemoveItem(ItemData removeItem, int amount = 1) => backpack.RemoveItem(removeItem, amount);

}
