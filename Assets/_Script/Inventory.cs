using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum EAmmoType 
{
    smallAmmo,
    largeAmmo,
    ShellAmmo
}

[System.Serializable]
public struct InventoryItem
{
    public InventoryItem(SOItem soItem, int amount)
    {
        SOItem = soItem;
        Amount = amount;
    }

    [field: SerializeField] public SOItem SOItem { get; set; }
    [field: SerializeField] public int Amount { get; set; }
}

public class Inventory : MonoBehaviour
{
    [Header("Item Inventory")]
    [field: SerializeField] public List<InventoryItem> InventoryList { get; private set; } = new List<InventoryItem>();

    // Consumable indexes list
    public List<int> consumableIndexes = new List<int>(); 
    // Selected consumable
    public int selectedConsumableIndex { get; set; } = 0;

    [Header("Ammo Inventory")]
    public int SmallAmmoAmount { get; private set; } = 0;
    public int LargeAmmoAmount { get; private set; } = 0;
    public int ShellAmmoAmount { get; private set; } = 0;

    public event Action OnItemAdded;
    public event Action OnItemRemoved;
    public event Action OnConsumableIndexUpdate;

    public delegate void AmmoUpdate();
    public event AmmoUpdate OnAmmoCountUpdate;

    private void Start()
    {
        SetConsumableList();
    }

    void Update()
    {
        // Use input input
        if (Input.GetKeyDown(KeyCode.F)) UseSelectedConsumable();

        // Scroll consumables input
        if (Input.mouseScrollDelta.y > 0f)
        {
            ChangeConsumableIndex(1);
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            ChangeConsumableIndex(-1);
        }
    }

    public int GetAmmoAmountByType(EAmmoType ammoType) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                return SmallAmmoAmount;
            case EAmmoType.largeAmmo:
                return LargeAmmoAmount;
            case EAmmoType.ShellAmmo:
                return ShellAmmoAmount;
            default:
                return 0;
        }
    }

    public void AddAmmo(EAmmoType ammoType, int amount) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                SmallAmmoAmount += amount;
                break;
            case EAmmoType.largeAmmo:
                LargeAmmoAmount += amount;
                break;
            case EAmmoType.ShellAmmo:
                ShellAmmoAmount += amount;
                break;
            default:
                Debug.LogError("Failed to add ammo");
                break;
        }

        OnAmmoCountUpdate?.Invoke();
    }

    public void RemoveAmmo(EAmmoType ammoType, int amount) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                SmallAmmoAmount -= amount;
                SmallAmmoAmount = Mathf.Clamp(SmallAmmoAmount, 0, 999);
                break;
            case EAmmoType.largeAmmo:
                LargeAmmoAmount -= amount;
                LargeAmmoAmount = Mathf.Clamp(SmallAmmoAmount, 0, 999);
                break;
            case EAmmoType.ShellAmmo:
                ShellAmmoAmount -= amount;
                ShellAmmoAmount = Mathf.Clamp(SmallAmmoAmount, 0, 999);
                break;
            default:
                break;
        }

        OnAmmoCountUpdate?.Invoke();
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            // Check if already have the item
            if (item.SOItem.itemName == InventoryList[i].SOItem.itemName)
            {
                // If have the item, check if you have the max amount
                if (InventoryList[i].SOItem.isStackable && InventoryList[i].Amount < InventoryList[i].SOItem.maxStack /*&& iteminventoryList.Count <= inventorySize*/)
                {
                    // Increase item quantity
                    InventoryItem inventoryItem = InventoryList[i];
                    inventoryItem.Amount += item.Amount;

                    // Set the new inventoty item with the correct amount
                    InventoryList[i] = inventoryItem;

                    SetConsumableList();
                    OnItemAdded?.Invoke();
                    return true;
                }
                else
                {
                    Debug.Log("You have the max amount of " + InventoryList[i].SOItem.itemName);
                    return false;
                }
            }
        }

        InventoryList.Add(new InventoryItem(item.SOItem, 1));
        SetConsumableList();
        OnItemAdded?.Invoke();
        return true;
    }

    public bool RemoveItem(SOItem item)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i].SOItem.itemName == item.itemName)
            {
                InventoryItem inventoryItem = InventoryList[i];
                inventoryItem.Amount--;

                InventoryList[i] = inventoryItem;

                if (InventoryList[i].Amount == 0)
                {
                    InventoryList.RemoveAt(i);
                }

                SetConsumableList();
                OnItemRemoved?.Invoke();
                return true;
            }
        }

        Debug.LogError("Cant Remove Item | Dont Have Item");
        return false;
    }

    private void SetConsumableList()
    {
        List<int> list = new List<int>();

        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i].SOItem.GetType() == typeof(SOConsumable))
            {
                list.Add(i);
            }
        }

        consumableIndexes = list;
    }

    private void ChangeConsumableIndex(int amount)
    {
        selectedConsumableIndex += amount;

        if (selectedConsumableIndex >= consumableIndexes.Count)
        {
            selectedConsumableIndex = 0;
        }
        else if (selectedConsumableIndex < 0)
        {
            if (consumableIndexes.Count == 0)
            {
                selectedConsumableIndex = 0;
            }
            else 
            {
                selectedConsumableIndex = consumableIndexes.Count - 1;
            } 
        }

        OnConsumableIndexUpdate?.Invoke();
    }

    private void UseSelectedConsumable()
    {
        if (consumableIndexes.Count == 0)
        {
            Debug.Log("No item to use");
            return;
        }

        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (i == consumableIndexes[selectedConsumableIndex])
            {
                if (InventoryList[i].SOItem.GetType() == typeof(SOConsumable))
                {
                    SOConsumable soConsumable = (SOConsumable)InventoryList[i].SOItem;

                    GetComponent<Health>().AddHealth(soConsumable.recoverHealth);

                    Debug.Log(InventoryList[i].SOItem.name + " used and removed");

                    RemoveItem(InventoryList[i].SOItem);
                    break;
                }
                else
                {
                    Debug.Log("Cant use, item is not consumable");
                }
            }
        }
    }

    public bool HaveItem(SOItem item)
    {
        foreach (InventoryItem i in InventoryList)
        {
            if (item.name == i.SOItem.name)
            {
                Debug.Log("Player has " + item.itemName + " in the inventory");
                return true;
            }
        }
        Debug.Log("Player has not " + item.itemName + " in the inventory");
        return false;
    }
}