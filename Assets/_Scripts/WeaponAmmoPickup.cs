using UnityEngine;

public class WeaponAmmoPickup : Interact
{
    public override void Interacting(Transform Interactor)
    {
        Inventory inventory;

        if ((inventory = Interactor.GetComponent<Inventory>()) && inventory.AddItem(GetComponent<Item>()))
        {
            Destroy(GetComponent<Name>());

            //Despawn the gun mag (if they have)
            Weapon weapon = GetComponentInChildren<Weapon>();
            if (weapon.GetAmmoMeshTransform() != null) 
            {
                Destroy(weapon.GetAmmoMeshTransform().gameObject);
            } 

            Debug.Log("<b><color=magenta>" + Interactor.name + "</color></b> Picked ammo from <b><color=cyan>" + transform.name + "</color></b>");

            Destroy(this);
        }
    }
}
