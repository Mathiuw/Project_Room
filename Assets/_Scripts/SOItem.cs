using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class SOItem : ScriptableObject
{
    public ItemType itemType;

    public enum ItemType
    {
        redKeycard,
        blueKeycard,
        yellowKeycard,
        greenKeycard,
        reloadWeapon,
        consumable,
    }

    [Header("Name")]
    public string itemName;

    [Header("Sprite and mesh")]
    public Sprite hotbarSprite;
    public GameObject itemPrefab;

    [Header("Stack")]
    public bool isStackable;
    public int maxStack;

    [Header("Consumable effects")]
    public int recoverHealth;
}
