using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WeaponShoot))]
public class WeaponReload : MonoBehaviour
{
    public bool isReloading { get; private set; } = false;
    SOWeapon weaponSO;
    Ammo ammo;

    public event Action onReloadStart;
    public event Action onReloadEnd;

    IEnumerator Start() 
    {
        yield return new WaitForEndOfFrame();

        weaponSO = GetComponent<Weapon>().weaponSO;
        ammo = GetComponent<Ammo>();
    }

    public void SetReload(bool b) => isReloading = b;

    public IEnumerator Reload(Inventory inventory = null) 
    {
        if (inventory != null) 
        {
            if (!inventory.HaveItem(weaponSO.reloadItem)) yield break;
            inventory.RemoveItem(weaponSO.reloadItem);
        }

        SetReload(true);
        onReloadStart?.Invoke();

        yield return new WaitForSeconds(weaponSO.reloadTime);

        ammo.AddAmmo(ammo.maxAmmo);
        SetReload(false);
        onReloadEnd?.Invoke();
        yield break;
    }
}
