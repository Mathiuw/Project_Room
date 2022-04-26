using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 5;

    public List<SetItem> inventory = new List<SetItem>();

    public bool CheckAndAddItem(SetItem item)
    {
        foreach (SetItem i in inventory)
        {
            if (item.item.itemName == i.item.itemName)
            {
                if (item.item.isStackable && i.amount < i.item.maxStack && inventory.Count <= inventorySize)
                {
                    i.amount++;
                    return true;
                }
            }
        }
        if (inventory.Count< inventorySize)
        {
            inventory.Add(item);
            return true;
        }
        else
        {
            Debug.Log("Inventory full");
            return false;
        }
    }

    public bool CheckAndRemoveItem(SetItem item)
    {
        foreach (SetItem i in inventory)
        {
            if (item.item.itemName == i.item.itemName)
            {
                if (item.item.isStackable && i.amount < i.item.maxStack && inventory.Count <= inventorySize)
                {
                    i.amount--;
                    return true;
                }
            }
        }
        if (inventory.Count < inventorySize)
        {
            inventory.Remove(item);
            return true;
        }
        else
        {
            Debug.LogError("Cant Remove Item");
            return false;
        }
    }

    public bool HasItemOnIndex(Items item)
    {
        foreach (SetItem i in inventory)
        {
            if (inventory.IndexOf(i) == SelectItem.index)
            {
                if (item.itemType == i.item.itemType)
                {
                    Debug.Log("Player has " + item.itemType + "in the inventory");
                    return true;
                }
            }
        }
        Debug.Log("Player has not " + item.itemName + "in the inventory");
        return false;
    }

    public bool HasItemOnInventory(Items item)
    {
        foreach (SetItem i in inventory)
        {
            if (item.itemType == i.item.itemType)
            {
                Debug.Log("Player has " + item.itemType + "in the inventory");
                return true;
            }
        }
        Debug.Log("Player has not " + item + "in the inventory");
        return false;
    }
}
