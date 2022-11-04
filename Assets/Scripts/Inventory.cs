using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int inventorySize = 5;
    public int InventorySize { get => inventorySize; set => inventorySize = value; }

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

    public bool CheckAndRemoveItem(Items item)
    {
        foreach (SetItem i in inventory)
        {
            if (item.itemName == i.item.itemName)
            {
                if (item.isStackable && i.amount > 1)
                {
                    i.amount--;
                    return true;
                }
                else
                {
                    inventory.Remove(i);
                    return true;
                }
            }
        }
        Debug.LogError("Cant Remove Item / Dont Have Item");
        return false;
    }

    public bool HasItemOnIndex(Items item)
    {
        foreach (SetItem i in inventory)
        {
            if (inventory.IndexOf(i) == SelectItem.index && item.itemName == i.item.itemName)
            {
                Debug.Log("Player has " + item.itemType + "in the Index");
                return true;
            }
        }
        Debug.LogError("Player has not " + item.itemName + "in the Index");
        return false;
    }

    public bool HasItemOnInventory(Items item)
    {
        foreach (SetItem i in inventory)
        {
            if (item.name == i.item.name)
            {
                Debug.Log("Player has " + item.itemType + "in the inventory");
                return true;
            }
        }
        Debug.Log("Player has not " + item + "in the inventory");
        return false;
    }
}
