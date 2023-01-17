using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(ShootGun))]
public class ReloadGun : MonoBehaviour
{
    bool canDo = true;

    [Header("Reload Config")]
    [SerializeField] Items reloadMag;
    public float reloadTime { get; private set; } = 2;
    public bool reloading { get; private set; } = false;

    ShootGun shootGun;

    public event Action ReloadStarted;
    public event Action ReloadEnded;

    void Awake() => shootGun = GetComponent<ShootGun>();

    //Ação de Recarregar a Arma
    public void Reloading()
    {
        if (!canDo) return;

        if (Player.Instance.Inventory.HasItemOnInventory(reloadMag) && !Player.Instance.Animator.GetBool("isAiming")
            && !Player.Instance.Animator.GetBool("isShooting") && !reloading && shootGun.ammo != shootGun.maximumAmmo)
        {
            Player.Instance.Inventory.CheckAndRemoveItem(reloadMag);
            Player.Instance.UIInventory.RefreshInventory();
            StartCoroutine(ReloadCoroutine());
        }
    }

    //Coroutine Para Determinar o Tempo de Recarregamento da Arma
    IEnumerator ReloadCoroutine()
    {
        string gunName = GetComponent<Name>().text;

        Debug.Log("Reload Start");
        ReloadStarted?.Invoke();
        Player.Instance.Animator.SetBool("isShooting", false);
        Player.Instance.Animator.SetBool("isAiming", false);
        reloading = true;
        Player.Instance.Animator.Play( "Weapon Start Reloading" , 0);

        yield return new WaitForSeconds(reloadTime);

        Player.Instance.Animator.SetTrigger("ReloadEnd");
        shootGun.AddAmmo(shootGun.maximumAmmo);
        reloading = false;
        ReloadEnded?.Invoke();
        Debug.Log("Reload End");
        yield break;
    }
}
