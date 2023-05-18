using UnityEngine;

public class WeaponAmmoPickup : Interact
{
    public override void Interacting(Transform t)
    {
        Inventory inventory;

        if ((inventory = t.GetComponent<Inventory>()) && inventory.AddItem(GetComponent<Item>()))
        {
            UI_Inventory.instance.RefreshInventory();

            Destroy(GetComponent<Name>());

            //Despawn the gun mag (if they have)
            WeaponLocations weaponLocations = GetComponentInChildren<WeaponLocations>();
            if(weaponLocations.GetAmmoMeshTransform() != null) Destroy(weaponLocations.GetAmmoMeshTransform().gameObject);

            Debug.Log("<b><color=magenta>" + t.name + "</color></b> Picked ammo from <b><color=cyan>" + transform.name + "</color></b>");

            Destroy(this);
        }
    }
}
