using System;
using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    Inventory inventory;
    PlayerDrop playerDrop;

    [Header("Drop item")]
    [SerializeField] GameObject itemPrefab;

    public Action<Transform> onDrop;

    void Awake() 
    {
        inventory = GetComponent<Inventory>();   
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.F)) UseItem();

        if (Input.GetKeyDown(KeyCode.Q)) DropItem();
    }

    public void UseItem()
    {
        foreach (Item item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == UI_SelectItem.index && item.item.itemType == SOItem.ItemType.consumable)
            {
                GetComponent<Health>().AddHealth(item.item.recoverHealth);

                if (item.amount > 1) item.amount--;
                else inventory.inventory.Remove(item);

                if (UI_Inventory.instance != null) UI_Inventory.instance.RefreshInventory();
                Debug.Log(item.item.name + " used and removed");
                break;
            }
        }
    }

    public void DropItem()
    {
        SpawnDropItem();
        if(UI_Inventory.instance != null) UI_Inventory.instance.RefreshInventory();
        Debug.Log("Item Droped");
    }

    void SpawnDropItem()
    {
        foreach (Item item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == UI_SelectItem.index)
            {
                if (item.amount == 1) inventory.inventory.RemoveAt(UI_SelectItem.index);
                else item.amount--;
                GameObject itemSpawned = Instantiate(itemPrefab, transform.position, transform.rotation);
                itemSpawned.GetComponent<Item>().item = item.item;
                onDrop?.Invoke(itemSpawned.transform);
                break;
            }
        }
    }
}