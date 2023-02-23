using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    protected override IEnumerator PickUpWeapon(Transform gun)
    {
        gun.SetParent(gunHolder);
        gun.transform.localScale = Vector3.one;
        currentWeapon = gun.GetComponent<weapon>();
        currentWeapon.OnBeingHold(true); 
        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        StartCoroutine(currentWeapon.reloadGun.Reload(0));
        yield break;
    }

    public override void DropGun()
    {
        currentWeapon.transform.SetParent(null);
        currentWeapon.OnBeingHold(false);
        currentWeapon = null;
    }
}
