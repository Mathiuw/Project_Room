using UnityEngine;

public class AmmoDrop : MonoBehaviour, IInteractable
{
    [SerializeField]EAmmoType ammoType;
    [SerializeField] int ammoAmount = 1;

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
