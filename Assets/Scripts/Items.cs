using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Items : ScriptableObject
{
    public ItemType itemType;

    public enum ItemType
    {
        redKeycard,
        blueKeycard,
        yellowKeycard,
        greenKeycard,
        PistolMag,
        RecoverHealth,
        RecoverStamina,
    }

    [Header("Name of the item")]
    public string itemName;

    [Header("Sprite and mesh")]
    public Sprite hotbarSprite;
    public GameObject itemPrefab;

    [Header("Stack")]
    public bool isStackable;
    public int maxStack;

    [Header("Item effects")]
    public bool isConsumable;
    public int recoverHealth;
    public float recoverStamina;
}
