using UnityEngine;

public class EnemyAmmoDrop : MonoBehaviour, IInteractable
{
    public void Interact(Transform interactor)
    {
        Inventory inventory;

        if ((inventory = interactor.GetComponent<Inventory>()) && inventory.AddItem(GetComponent<Item>()))
        {
            Destroy(GetComponent<ShowNameToHUD>());

            //Despawn the gun mag (if they have)
            Weapon weapon = GetComponentInChildren<Weapon>();
            if (weapon.GetAmmoMeshTransform() != null)
            {
                Destroy(weapon.GetAmmoMeshTransform().gameObject);
            }

            Debug.Log("<b><color=magenta>" + interactor.name + "</color></b> Picked ammo <b><color=cyan>");

            Destroy(this);
        }
    }

}
