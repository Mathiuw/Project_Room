using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(ShootGun))]
public class ReloadGun : MonoBehaviour
{
    [Header("Reload Config")]
    public Items reloadMag;

    [field: SerializeField] public float reloadTime { get; private set; } = 2;

    public bool isReloading { get; private set; } = false;

    ShootGun shootGun;

    public event Action onReloadStart;
    public event Action onReloadEnd;

    void Awake() 
    {
        shootGun = GetComponent<ShootGun>();
    }  

    public void SetReload(bool b) => isReloading = b;

    public IEnumerator Reload(float time, Inventory inventory = null) 
    {
        if (inventory != null) 
        {
            if (!inventory.HaveItem(reloadMag)) yield break;
            inventory.RemoveItem(reloadMag);
        }

        SetReload(true);
        onReloadStart?.Invoke();

        yield return new WaitForSeconds(time);

        shootGun.AddAmmo(shootGun.maximumAmmo);
        SetReload(false);
        onReloadEnd?.Invoke();
        yield break;
    }
}
