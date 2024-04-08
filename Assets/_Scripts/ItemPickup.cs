using UnityEngine;

public class ItemPickup : Interact
{
    public override void Interacting(Transform t)
    {
        Inventory inventory;

        if ((inventory = t.GetComponent<Inventory>()) && inventory.AddItem(GetComponent<Item>()))
        {
            Destroy(gameObject);
            Debug.Log("Picked item");
        }
    }
}
