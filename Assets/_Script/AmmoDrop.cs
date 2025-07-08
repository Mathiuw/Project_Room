using UnityEngine;

public class AmmoDrop : MonoBehaviour, IInteractable, IUIName
{
    [SerializeField]EAmmoType ammoType;
    [SerializeField] int ammoAmount = 1;

    public string ReadName => "Pickup Ammo";

    public void Interact(Transform interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory)
        {
            inventory.AddAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("interactor does not have inventory");
        }
    }
    
}
