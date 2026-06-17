using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemData : ScriptableObject
{
    public enum ItemType { KeyItem, Comsumable };
    public ItemType itemType;
    public Sprite icon;
}