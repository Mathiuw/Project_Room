using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    protected override IEnumerator PickUpWeapon(Transform gun)
    {
        gun.SetParent(weaponHolder);
        gun.transform.localScale = Vector3.one;
        Weapon = gun.GetComponent<Weapon>();
        Weapon.SetHoldState(true, transform);

        Destroy(Weapon.GetComponent<Name>());
        
        isHoldingWeapon = true;

        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> picked up gun");

        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!isHoldingWeapon) yield break;

        Weapon.AddAmmo(Weapon.GetMaxAmmo());
        
        yield break;
    }

    public override void DropWeapon()
    {
        //Destroy components
        Destroy(Weapon.GetComponent<Weapon>());

        //set Ammo drop for player
        Item ammoDrop = Weapon.AddComponent<Item>();
        ammoDrop.SOItem = Weapon.GetReloadItem();
        ammoDrop.amount = 1;
        //Ammo drop pickup
        Weapon.AddComponent<WeaponAmmoPickup>();
        //Ammo drop name
        Name ammoDropName = Weapon.AddComponent<Name>();
        ammoDropName.SetText("Pickup ammo");

        //Sets weapon state and current weapon NULL
        Weapon.transform.SetParent(null);
        Weapon.SetHoldState(false, null);
        Weapon = null;

        Debug.Log(name + " dropped weapon");
    }
}
