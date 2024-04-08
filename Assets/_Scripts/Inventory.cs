using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int inventorySize = 5;
    public int InventorySize { get => inventorySize; set => inventorySize = value; }

    public List<Item> inventory = new List<Item>();

    public event Action OnItemAdded;
    public event Action OnItemRemoved;

    public bool AddItem(Item item)
    {
        foreach (Item i in inventory)
        {
            if (item.SOItem.itemName == i.SOItem.itemName)
            {
                if (item.SOItem.isStackable && i.amount < i.SOItem.maxStack && inventory.Count <= inventorySize)
                {
                    i.amount++;
                    OnItemAdded.Invoke();
                    return true;
                }
            }
        }
        if (inventory.Count< inventorySize)
        {
            inventory.Add(item);
            OnItemAdded.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Inventory full");
            return false;
        }
    }

    public bool RemoveItem(SOItem item)
    {
        foreach (Item i in inventory)
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
                    inventory.Remove(i);
                    OnItemRemoved.Invoke();
                    return true;
                }
            }
        }
        Debug.LogError("Cant Remove Item / Dont Have Item");
        return false;
    }

    public bool HaveItemSelected(SOItem item)
    {
        foreach (Item i in inventory)
        {
            if (inventory.IndexOf(i) == UI_SelectItem.index && item.itemName == i.SOItem.itemName)
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
        foreach (Item i in inventory)
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
