using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WeaponShoot))]
public class ReloadGun : MonoBehaviour
{
    public float reloadTime { get; private set; }
    public bool isReloading { get; private set; } = false;

    Items reloadItem;

    public event Action onReloadStart;
    public event Action onReloadEnd;

    public void SetAttributes(float reloadTime, Items reloadItem) 
    {
        this.reloadTime = reloadTime;
        this.reloadItem = reloadItem;
    } 

    public void SetReload(bool b) => isReloading = b;

    public IEnumerator Reload(float time, Ammo ammo, Inventory inventory = null) 
    {
        if (inventory != null) 
        {
            if (!inventory.HaveItem(reloadItem)) yield break;
            inventory.RemoveItem(reloadItem);
        }

        SetReload(true);
        onReloadStart?.Invoke();

        yield return new WaitForSeconds(time);

        ammo.AddAmmo(ammo.maxAmmo);
        SetReload(false);
        onReloadEnd?.Invoke();
        yield break;
    }
}
