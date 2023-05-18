using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    protected override IEnumerator PickUpWeapon(Transform gun)
    {
        gun.SetParent(weaponHolder);
        gun.transform.localScale = Vector3.one;
        currentWeapon = gun.GetComponent<Weapon>();
        currentWeapon.SetHoldState(true, transform);

        Destroy(currentWeapon.GetComponent<Name>());
        currentWeapon.AddComponent<WeaponShoot>();
        currentWeapon.AddComponent<WeaponParticlesManager>();
        
        isHoldingWeapon = true;

        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> picked up gun");

        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!isHoldingWeapon) yield break;

        WeaponAmmo weaponAmmo = currentWeapon.GetComponent<WeaponAmmo>();

        weaponAmmo.AddAmmo(weaponAmmo.maxAmmo);
        
        yield break;
    }

    public override void DropWeapon()
    {
        //Destroy components
        Destroy(currentWeapon.GetComponent<Weapon>());
        Destroy(currentWeapon.GetComponent<WeaponShoot>());
        Destroy(currentWeapon.GetComponent<WeaponAmmo>());
        Destroy(currentWeapon.GetComponent<WeaponParticlesManager>());

        //set Ammo drop for player
        Item ammoDrop = currentWeapon.AddComponent<Item>();
        ammoDrop.item = currentWeapon.weaponSO.reloadItem;
        ammoDrop.amount = 1;
        //Ammo drop pickup
        currentWeapon.AddComponent<WeaponAmmoPickup>();
        //Ammo drop name
        Name ammoDropName = currentWeapon.AddComponent<Name>();
        ammoDropName.SetText("Pickup ammo");

        //Sets weapon state and current weapon NULL
        currentWeapon.transform.SetParent(null);
        currentWeapon.SetHoldState(false, null);
        currentWeapon = null;

        Debug.Log(name + " dropped weapon");
    }
}
