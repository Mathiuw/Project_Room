using UnityEngine;

public abstract class SOItem : ScriptableObject
{
    [Header("Name")]
    public string itemName;

    [Header("Sprite and mesh")]
    public Sprite hotbarSprite;
    public GameObject itemPrefab;

    [Header("Stack")]
    public bool isStackable;
    public int maxStack;


}
