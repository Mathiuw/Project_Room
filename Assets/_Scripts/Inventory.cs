using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int inventorySize = 5;
    public int InventorySize { get => inventorySize; set => inventorySize = value; }

    public List<Item> inventory = new List<Item>();

    public bool AddItem(Item item)
    {
        foreach (Item i in inventory)
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

    public bool RemoveItem(SOItem item)
    {
        foreach (Item i in inventory)
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

    public bool HaveItemSelected(SOItem item)
    {
        foreach (Item i in inventory)
        {
            if (inventory.IndexOf(i) == UI_SelectItem.index && item.itemName == i.item.itemName)
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
            if (item.name == i.item.name)
            {
                Debug.Log("Player has " + item.itemName + " in the inventory");
                return true;
            }
        }
        Debug.Log("Player has not " + item.itemName + " in the inventory");
        return false;
    }
}
