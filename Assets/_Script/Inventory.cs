using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EAmmoType 
{
    smallAmmo,
    largeAmmo,
    ShellAmmo
}

public class Inventory : MonoBehaviour
{
    [Header("Item Inventory")]
    //[SerializeField] int inventorySize = 5;
    public List<Item> itemInventoryList = new List<Item>();
    int index = 0;

    //[Header("Drop item")]
    //[SerializeField] float dropForce = 3.5f;

    [Header("Ammo Inventory")]
    int smallAmmoAmount = 0;
    int largeAmmoAmount = 0;
    int shellAmmoAmount = 0;

    public event Action OnItemAdded;
    public event Action OnItemRemoved;

    public delegate void AmmoUpdate();
    public event AmmoUpdate OnAmmoUpdate;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) UseItem();

        //if (Input.GetKeyDown(KeyCode.Q)) DropItem();
    }

    public int GetSmallAmmoAmount() { return smallAmmoAmount; }

    public int GetLargeAmmoAmount() { return largeAmmoAmount; }

    public int GetShellAmmoAmount() { return shellAmmoAmount; }

    public int GetAmmoAmountByType(EAmmoType ammoType) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                return smallAmmoAmount;
            case EAmmoType.largeAmmo:
                return largeAmmoAmount;
            case EAmmoType.ShellAmmo:
                return shellAmmoAmount;
            default:
                return 0;
        }
    }

    public void AddAmmo(EAmmoType ammoType, int amount) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                smallAmmoAmount += amount;
                break;
            case EAmmoType.largeAmmo:
                largeAmmoAmount += amount;
                break;
            case EAmmoType.ShellAmmo:
                shellAmmoAmount += amount;
                break;
            default:
                Debug.LogError("Failed to add ammo");
                break;
        }

        OnAmmoUpdate?.Invoke();
    }

    public void RemoveAmmo(EAmmoType ammoType, int amount) 
    {
        switch (ammoType)
        {
            case EAmmoType.smallAmmo:
                smallAmmoAmount -= amount;
                smallAmmoAmount = Mathf.Clamp(smallAmmoAmount, 0, 999);
                break;
            case EAmmoType.largeAmmo:
                largeAmmoAmount -= amount;
                largeAmmoAmount = Mathf.Clamp(smallAmmoAmount, 0, 999);
                break;
            case EAmmoType.ShellAmmo:
                shellAmmoAmount -= amount;
                shellAmmoAmount = Mathf.Clamp(smallAmmoAmount, 0, 999);
                break;
            default:
                break;
        }

        OnAmmoUpdate?.Invoke();
    }

    public bool AddItem(Item item)
    {
        foreach (Item i in itemInventoryList)
        {
            if (item.SOItem.itemName == i.SOItem.itemName)
            {
                if (item.SOItem.isStackable && i.amount < i.SOItem.maxStack /*&& iteminventoryList.Count <= inventorySize*/)
                {
                    i.amount++;
                    OnItemAdded.Invoke();
                    return true;
                }
                else
                {
                    Debug.Log("You have the max amount of " + item.SOItem.itemName);
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

        itemInventoryList.Add(item);
        OnItemAdded.Invoke();
        return true;
    }

    public void UseItem()
    {
        foreach (Item item in itemInventoryList)
        {           
            if (itemInventoryList.IndexOf(item) == UI_SelectItem.index)
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
        foreach (Item i in itemInventoryList)
        {
            if (item.itemName == i.SOItem.itemName)
            {
                if (item.isStackable && i.amount > 1)
                {
                    i.amount--;
                    OnItemRemoved.Invoke();
                    return true;
                }
                else
                {
                    itemInventoryList.Remove(i);
                    OnItemRemoved.Invoke();
                    return true;
                }
            }
        }
        Debug.LogError("Cant Remove Item / Dont Have Item");
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
        foreach (Item i in itemInventoryList)
        {
            if (itemInventoryList.IndexOf(i) == index && item.itemName == i.SOItem.itemName)
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
        foreach (Item i in itemInventoryList)
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
