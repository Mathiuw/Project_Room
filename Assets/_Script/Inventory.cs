using System;
using System.Collections.Generic;
using UnityEngine;

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
    public List<int> consumableIndexes = new List<int>(); 
    public int selectedConsumableIndex { get; set; } = 0;

    [Header("Ammo Inventory")]
    public int SmallAmmoAmount { get; private set; } = 0;
    public int LargeAmmoAmount { get; private set; } = 0;
    public int ShellAmmoAmount { get; private set; } = 0;

    public event Action OnItemAdded;
    public event Action OnItemRemoved;

    public delegate void AmmoUpdate();
    public event AmmoUpdate OnAmmoCountUpdate;

    public delegate void NextConsumableSelection();
    public event NextConsumableSelection OnConsumableListUpdate;

    private void Start()
    {
        SetConsumableList();
    }

    void Update()
    {
        // Use input input
        if (Input.GetKeyDown(KeyCode.F)) UseItem();

        // Scroll consumables input
        if (Input.mouseScrollDelta.y > 0f)
        {
            AddSelectedConsumableIndex(1);
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            AddSelectedConsumableIndex(-1);
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
                    // Invoke OnItemAdded 
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

    private void UseItem()
    {
        foreach (InventoryItem item in InventoryList)
        {           
            if (InventoryList.IndexOf(item) == consumableIndexes[selectedConsumableIndex])
            {
                SOConsumable soConsumable = (SOConsumable)item.SOItem;

                if (soConsumable != null) 
                {
                    GetComponent<Health>().AddHealth(soConsumable.recoverHealth);

                    RemoveItem(item.SOItem);

                    Debug.Log(item.SOItem.name + " used and removed");
                    break;                 
                }
                else
                {
                    Debug.Log("Cant use, item is not consumable");
                }
            }
        }
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
            if ((SOConsumable)InventoryList[i].SOItem)
            {
                list.Add(i);
            }
        }

        consumableIndexes = list;
        OnConsumableListUpdate?.Invoke();
    }

    private void AddSelectedConsumableIndex(int amount) 
    {
        selectedConsumableIndex += amount;

        if (selectedConsumableIndex >= consumableIndexes.Count)
        {
            selectedConsumableIndex = 0;
        }
        else if (selectedConsumableIndex < 0)
        {
            selectedConsumableIndex = consumableIndexes.Count - 1;
        }

        OnConsumableListUpdate?.Invoke();
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