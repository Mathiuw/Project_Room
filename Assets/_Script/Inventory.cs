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
    //[SerializeField] int inventorySize = 5;
    [field: SerializeField] public List<InventoryItem> ItemInventoryList { get; private set; } = new List<InventoryItem>();
    int index = 0;

    //[Header("Drop item")]
    //[SerializeField] float dropForce = 3.5f;

    [Header("Ammo Inventory")]
    public int SmallAmmoAmount { get; private set; } = 0;
    public int LargeAmmoAmount { get; private set; } = 0;
    public int ShellAmmoAmount { get; private set; } = 0;

    public event Action OnItemAdded;
    public event Action OnItemRemoved;

    public delegate void AmmoUpdate();
    public event AmmoUpdate OnAmmoCountUpdate;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) UseItem();

        //if (Input.GetKeyDown(KeyCode.Q)) DropItem();
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
        for (int i = 0; i < ItemInventoryList.Count; i++)
        {
            // Check if already have the item
            if (item.SOItem.itemName == ItemInventoryList[i].SOItem.itemName)
            {
                // If have the item, check if you have the max amount
                if (ItemInventoryList[i].SOItem.isStackable && ItemInventoryList[i].Amount < ItemInventoryList[i].SOItem.maxStack /*&& iteminventoryList.Count <= inventorySize*/)
                {
                    // Increase item quantity
                    InventoryItem inventoryItem = ItemInventoryList[i];
                    inventoryItem.Amount += item.Amount;

                    // Set the new inventoty item with the correct amount
                    ItemInventoryList[i] = inventoryItem;

                    // Invoke OnItemAdded Event
                    OnItemAdded?.Invoke();
                    return true;
                }
                else
                {
                    Debug.Log("You have the max amount of " + ItemInventoryList[i].SOItem.itemName);
                    return false;
                }
            }
        }
        //if (iteminventoryList.Count< inventorySize)
        //{
        //    iteminventoryList.Add(item);
        //    OnItemAdded.Invoke();
        //    return true;
        //}
        //else
        //{
        //    Debug.Log("Inventory full");
        //    return false;
        //}

        ItemInventoryList.Add(new InventoryItem(item.SOItem, 1));
        OnItemAdded?.Invoke();
        return true;
    }

    public void UseItem()
    {
        foreach (InventoryItem item in ItemInventoryList)
        {           
            if (ItemInventoryList.IndexOf(item) == UI_SelectItem.index)
            {
                SOConsumable soConsumable = (SOConsumable)item.SOItem;

                if (soConsumable != null) 
                {
                    GetComponent<Health>().AddHealth(soConsumable.recoverHealth);

                    RemoveItem(item.SOItem);

                    Debug.Log(item.SOItem.name + " used and removed");
                    break;                 
                }
            }
        }
    }

    public bool RemoveItem(SOItem item)
    {
        for (int i = 0; i < ItemInventoryList.Count; i++)
        {
            if (ItemInventoryList[i].SOItem.itemName == item.itemName)
            {
                InventoryItem inventoryItem = ItemInventoryList[i];
                inventoryItem.Amount--;

                ItemInventoryList[i] = inventoryItem;

                if (ItemInventoryList[i].Amount == 0)
                {
                    ItemInventoryList.RemoveAt(i);
                }

                OnItemRemoved.Invoke();
                return true;
            }
        }

        //foreach (InventoryItem i in itemInventoryList)
        //{
        //    if (item.itemName == i.SOItem.itemName)
        //    {
        //        if (item.isStackable && i.Amount > 1)
        //        {
        //            i.amount--;
        //            OnItemRemoved.Invoke();
        //            return true;
        //        }
        //        else
        //        {
        //            itemInventoryList.Remove(i);
        //            OnItemRemoved.Invoke();
        //            return true;
        //        }
        //    }
        //}
        Debug.LogError("Cant Remove Item | Dont Have Item");
        return false;
    }

    //public void DropItem()
    //{
    //    foreach (Item item in iteminventoryList)
    //    {
    //        if (iteminventoryList.IndexOf(item) == UI_SelectItem.index)
    //        {
    //            //Remove item from inventory
    //            RemoveItem(item.SOItem);

    //            //Intantiate and set item
    //            //SpawnItem itemSpawned = Instantiate(spawnItemPrefab, transform.position, transform.rotation);
    //            //itemSpawned.itemSO = item.SOItem;

    //            //Set item transform
    //            //itemSpawned.transform.localPosition = transform.position;
    //            //itemSpawned.transform.rotation = transform.rotation;

    //            //Apply force to item
    //            //Rigidbody dropRigidbody = itemSpawned.GetComponent<Rigidbody>();
    //            //dropRigidbody.AddForce(transform.forward * dropForce, ForceMode.VelocityChange);

    //            Debug.Log("Item Droped");
    //            break;
    //        }
    //    }
    //}

    public bool HaveItemSelected(SOItem item)
    {
        foreach (InventoryItem i in ItemInventoryList)
        {
            if (ItemInventoryList.IndexOf(i) == index && item.itemName == i.SOItem.itemName)
            {
                Debug.Log("Player has " + item.itemName + " in the Index");
                return true;
            }
        }
        Debug.LogError("Player has not " + item.itemName + " in the Index");
        return false;
    }

    public bool HaveItem(SOItem item)
    {
        foreach (InventoryItem i in ItemInventoryList)
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
