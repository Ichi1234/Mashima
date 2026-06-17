using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Backpack backpack;

    public static ItemManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StoreItem(Item item)
    {
        backpack.StoreItem(item);
        Debug.Log("Amount of " + item.name + " are " + backpack.GetItem(item));
    }

    public int GetItem(Item item) => backpack.GetItem(item);
}
