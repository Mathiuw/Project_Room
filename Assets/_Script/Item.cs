using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public SOItem SOItem;
    public int amount = 1;

    public void Interact(Transform interactor)
    {
        Inventory inventory;

        if ((inventory = interactor.GetComponent<Inventory>()) && inventory.AddItem(GetComponent<Item>()))
        {
            Debug.Log("Picked " + SOItem.name);
            Destroy(gameObject);
        }
    }
}
