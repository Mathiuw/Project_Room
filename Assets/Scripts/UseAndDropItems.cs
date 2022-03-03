using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAndDropItems : MonoBehaviour
{
    [Header("Pickup item")]
    [SerializeField] private float rayLenght;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask itemMask;
    Inventory inventory;
    UI_Inventory uiInventory;

    [Header("Drop item")]
    [SerializeField] private GameObject itemPrefab;

    private void Awake()
    {
        GameObject playerRoot = transform.parent.gameObject;

        inventory = GetComponent<Inventory>();
        uiInventory = playerRoot.GetComponentInChildren<UI_Inventory>();
    }

    private void Update()
    {
        DropItem();
        UseItem();
        pickupItem();
    }

    private void pickupItem()
    {
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, rayLenght, itemMask))
            {             
                if (hit.transform.GetComponent<SetItem>() && inventory.AddItem(hit.transform.GetComponent<SetItem>()))
                {
                    uiInventory.RefreshInventory();
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Picked item");
                }
            }
        }
    }

    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (SetItem item in inventory.inventory)
            {
                if (inventory.inventory.IndexOf(item) == SelectItem.index)
                {
                    if (item.item.isConsumable)
                    {
                        Sprint.playerStamina += item.item.recoverStamina;
                        Health.AddHealth(item.item.recoverHealth);
                        if (item.amount > 1)
                        {
                            item.amount--;
                        }
                        else
                        {
                            inventory.inventory.Remove(item);
                        }
                        uiInventory.RefreshInventory();
                        Debug.Log( item.item.name + " used and removed");
                    }
                    else
                    {
                        Debug.Log("Cant use item");
                    }
                    break;
                }
            }
        }
    }

    private void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnDropItem();
            uiInventory.RefreshInventory();
            Debug.Log("Drop item");
        }
    }

    private void SpawnDropItem()
    {
        foreach (SetItem item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == SelectItem.index)
            {
                if (item.amount == 1)
                {
                    inventory.inventory.RemoveAt(SelectItem.index);
                }
                else
                {
                    item.amount--;
                }
                GameObject itemSpawned = Instantiate(itemPrefab, cameraTransform.position + cameraTransform.forward * 1.5f, cameraTransform.rotation);
                itemSpawned.GetComponent<SetItem>().item = item.item;
                itemSpawned.GetComponent<Rigidbody>().AddForce(cameraTransform.forward * 5, ForceMode.VelocityChange);
                break;
            }
        }
    }
}