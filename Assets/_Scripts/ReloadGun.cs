using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ShootGun))]
public class ReloadGun : MonoBehaviour
{
    public float reloadTime { get; private set; }
    public bool isReloading { get; private set; } = false;

    ShootGun shootGun;
    Items reloadItem;

    public event Action onReloadStart;
    public event Action onReloadEnd;

    public void SetAttributes(float reloadTime, Items reloadItem, ShootGun shootGun)
    {
        this.reloadTime = reloadTime;
        this.reloadItem = reloadItem;
        this.shootGun = shootGun;
    }

    public void SetReload(bool b) => isReloading = b;

    public IEnumerator Reload(float time, Inventory inventory = null) 
    {
        if (inventory != null) 
        {
            if (!inventory.HaveItem(reloadItem)) yield break;
            inventory.RemoveItem(reloadItem);
        }

        SetReload(true);
        onReloadStart?.Invoke();

        yield return new WaitForSeconds(time);

        shootGun.AddAmmo(shootGun.maxAmmo);
        SetReload(false);
        onReloadEnd?.Invoke();
        yield break;
    }
}
