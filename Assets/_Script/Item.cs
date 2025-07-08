using UnityEngine;

public class Item : MonoBehaviour, IInteractable, IUIName
{
    [field: SerializeField] public SOItem SOItem { get; private set; }
    [field: SerializeField] public int Amount { get; set; } = 1;

    public string ReadName => SOItem.itemName;

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
