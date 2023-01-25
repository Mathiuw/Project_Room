using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interact
{
    public override void Interacting(Transform t)
    {
        Inventory inventory;

        if ((inventory = t.GetComponent<Inventory>()) && inventory.CheckAndAddItem(GetComponent<SetItem>()))
        {
            UI_Inventory.instance.RefreshInventory();
            Destroy(gameObject);
            Debug.Log("Picked item");
        }
    }
}
