using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(ShootGun))]
public class ReloadGun : MonoBehaviour
{
    [Header("Reload Config")]
    [SerializeField] Items reloadMag;
    public float reloadTime { get; private set; } = 2;
    public bool reloading { get; private set; } = false;

    ShootGun shootGun;

    public event Action ReloadStarted;
    public event Action ReloadEnded;

    void Awake() 
    {
        shootGun = GetComponent<ShootGun>();
    } 

    //Ação de Recarregar a Arma
    public void Reloading(Inventory inventory,Animator playerAnimator)
    {
        if (inventory.HaveItem(reloadMag) && !playerAnimator.GetBool("isAiming")
            && !playerAnimator.GetBool("isShooting") && !reloading && shootGun.ammo != shootGun.maximumAmmo)
        {
            inventory.RemoveItem(reloadMag);
            UI_Inventory.instance.RefreshInventory();
            StartCoroutine(ReloadCoroutine(playerAnimator));
        }
    }

    //Coroutine Para Determinar o Tempo de Recarregamento da Arma
    IEnumerator ReloadCoroutine(Animator playerAnimator)
    {
        string gunName = GetComponent<Name>().text;

        Debug.Log("Start Reload");
        ReloadStarted?.Invoke();
        playerAnimator.SetBool("isShooting", false);
        playerAnimator.SetBool("isAiming", false);
        reloading = true;
        playerAnimator.Play("Start Reload", 0);

        yield return new WaitForSeconds(reloadTime);

        playerAnimator.SetTrigger("ReloadEnd");
        shootGun.AddAmmo(shootGun.maximumAmmo);
        reloading = false;
        ReloadEnded?.Invoke();
        Debug.Log("Reload End");
        yield break;
    }
}
