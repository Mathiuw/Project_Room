using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] int inventorySize = 5;
    public List<Item> inventoryList = new List<Item>();

    [Header("Drop item")]
    [SerializeField] SpawnItem spawnItemPrefab;
    [SerializeField] float dropForce = 3.5f;

    public event Action OnItemAdded;
    public event Action OnItemRemoved;

    public int GetInventorySize() { return inventorySize; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) UseItem();

        if (Input.GetKeyDown(KeyCode.Q)) DropItem();
    }

    public bool AddItem(Item item)
    {
        foreach (Item i in inventoryList)
        {
            if (item.SOItem.itemName == i.SOItem.itemName)
            {
                if (item.SOItem.isStackable && i.amount < i.SOItem.maxStack && inventoryList.Count <= inventorySize)
                {
                    i.amount++;
                    OnItemAdded.Invoke();
                    return true;
                }
            }
        }
        if (inventoryList.Count< inventorySize)
        {
            inventoryList.Add(item);
            OnItemAdded.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Inventory full");
            return false;
        }
    }

    public void UseItem()
    {
        foreach (Item item in inventoryList)
        {           
            if (inventoryList.IndexOf(item) == UI_SelectItem.index)
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
        foreach (Item i in inventoryList)
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
                    inventoryList.Remove(i);
                    OnItemRemoved.Invoke();
                    return true;
                }
            }
        }
        Debug.LogError("Cant Remove Item / Dont Have Item");
        return false;
    }

    public void DropItem()
    {
        foreach (Item item in inventoryList)
        {
            if (inventoryList.IndexOf(item) == UI_SelectItem.index)
            {
                //Remove item from inventory
                RemoveItem(item.SOItem);

                //Intantiate and set item
                SpawnItem itemSpawned = Instantiate(spawnItemPrefab, transform.position, transform.rotation);
                itemSpawned.itemSO = item.SOItem;

                //Set item transform
                itemSpawned.transform.localPosition = transform.position;
                itemSpawned.transform.rotation = transform.rotation;

                //Apply force to item
                Rigidbody dropRigidbody = itemSpawned.GetComponent<Rigidbody>();
                dropRigidbody.AddForce(transform.forward * dropForce, ForceMode.VelocityChange);

                Debug.Log("Item Droped");
                break;
            }
        }
    }

    public bool HaveItemSelected(SOItem item)
    {
        foreach (Item i in inventoryList)
        {
            if (inventoryList.IndexOf(i) == UI_SelectItem.index && item.itemName == i.SOItem.itemName)
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
        foreach (Item i in inventoryList)
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
