using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    protected override IEnumerator PickUpWeapon(Transform gun)
    {
        gun.SetParent(gunHolder);
        gun.transform.localScale = Vector3.one;
        currentWeapon = gun.GetComponent<Weapon>();
        currentWeapon.SetHoldState(true, transform);

        currentWeapon.AddComponent<WeaponShoot>();
        currentWeapon.AddComponent<WeaponParticlesManager>();

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
        Destroy(currentWeapon.GetComponent<WeaponShoot>());
        Destroy(currentWeapon.GetComponent<WeaponParticlesManager>());

        currentWeapon.transform.SetParent(null);
        currentWeapon.SetHoldState(false, null);
        currentWeapon = null;
    }
}
