using System.Collections;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    protected override IEnumerator PickUpWeapon(Transform gun)
    {
        yield return new WaitForEndOfFrame();

        gun.SetParent(gunHolder);
        gun.transform.localScale = Vector3.one;
        currentWeapon = gun.GetComponent<Weapon>();
        currentWeapon.SetHoldState(true, transform); 
        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        StartCoroutine(currentWeapon.reloadGun.Reload());
        yield break;
    }

    public override void DropWeapon()
    {
        currentWeapon.transform.SetParent(null);
        currentWeapon.SetHoldState(false, null);
        currentWeapon = null;
    }
}
