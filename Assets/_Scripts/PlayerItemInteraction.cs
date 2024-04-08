using UnityEngine;

public class PlayerItemInteraction : MonoBehaviour
{
    Inventory inventory;

    [Header("Drop item settings")]
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float dropForce = 3.5f;

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
            if (inventory.inventory.IndexOf(item) == UI_SelectItem.index && item.SOItem.itemType == SOItem.ItemType.consumable)
            {
                GetComponent<Health>().AddHealth(item.SOItem.recoverHealth);

                inventory.RemoveItem(item.SOItem);

                Debug.Log(item.SOItem.name + " used and removed");
                break;
            }
        }
    }

    public void DropItem()
    {
        foreach (Item item in inventory.inventory)
        {
            if (inventory.inventory.IndexOf(item) == UI_SelectItem.index)
            {
                //Remove item from inventory
                inventory.RemoveItem(item.SOItem);
            
                //Intantiate and set item
                GameObject itemSpawned = Instantiate(itemPrefab, transform.position, transform.rotation);
                itemSpawned.GetComponent<SpawnItem>().item = item.SOItem;

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
}