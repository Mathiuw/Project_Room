using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(ShootGun))]
public class ReloadGun : MonoBehaviour, ICanDo
{
    bool canDo = true;

    [Header("Reload Config")]
    [SerializeField] Items reloadMag;
    public float reloadTime { get; private set; } = 2;
    public bool reloading { get; private set; } = false;

    ShootGun shootGun;

    public delegate void Reload();
    public event Reload OnReloadStart;
    public event Reload OnReloadEnd;

    void Awake()
    {
        shootGun = GetComponent<ShootGun>();
        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    //Ação de Recarregar
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

    //Corotina para determinar o tempo de recarregamento da arma
    IEnumerator ReloadCoroutine()
    {
        string gunName = GetComponent<Name>().text;

        Debug.Log("Reload Start");
        OnReloadStart?.Invoke();
        Player.Instance.Animator.SetBool("isShooting", false);
        Player.Instance.Animator.SetBool("isAiming", false);
        reloading = true;
        Player.Instance.Animator.Play( "Weapon Start Reloading" , 0);

        yield return new WaitForSeconds(reloadTime);

        Player.Instance.Animator.SetTrigger("ReloadEnd");
        shootGun.AddAmmo(shootGun.maximumAmmo);
        reloading = false;
        OnReloadEnd?.Invoke();
        Debug.Log("Reload End");
        yield break;
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}
