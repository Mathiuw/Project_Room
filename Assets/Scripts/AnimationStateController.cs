using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using System;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] GameObject ammoUI;
    [SerializeField] Image ammoType;

    void Start() 
    {
        Player.Instance.WeaponPickup.onPickupCoroutineStart += SetWeaponAnimations;
        Player.Instance.WeaponPickup.onPickupCoroutineEnd += PickupWeaponAnimation;
        Player.Instance.WeaponPickup.onPickupCoroutineEnd += HoldWeaponAnimation;
        Player.Instance.WeaponPickup.weaponDrop += DropWeaponAnimation;
        Player.Instance.WeaponPickup.weaponDrop += HoldWeaponAnimation;

        HoldWeaponAnimation(transform);        
    }

    void Update() 
    {
        Player.Instance.Animator.SetFloat("RbVelocity", Player.Instance.RigidBody.velocity.magnitude);

        if (Player.Instance.WeaponPickup.IsholdingWeapon()) Player.Instance.GetPlayerGun().onShoot += ShootWeaponAnimation;
    }

    void SetWeaponAnimations(Transform gun) 
    {
        Player.Instance.Animator.runtimeAnimatorController = gun.GetComponent<WeaponAnimations>().WeaponOverrideController;
    }

    void ShootWeaponAnimation() => Player.Instance.Animator.SetTrigger("isShooting");

    void HoldWeaponAnimation(Transform gun)
    {
        if (Player.Instance.WeaponPickup.IsholdingWeapon())
        {
            Player.Instance.Animator.SetBool("isHoldingWeapon", true);
            ammoUI.SetActive(true);
            ammoType.gameObject.SetActive(true);
            return;
        }

        Player.Instance.Animator.SetBool("isHoldingWeapon", false);
        ammoUI.SetActive(false);
        ammoType.gameObject.SetActive(false);
    }

    void PickupWeaponAnimation(Transform gun) => Player.Instance.Animator.SetBool("isHoldingWeapon", true);

    void DropWeaponAnimation(Transform gun) 
    {
        Player.Instance.Animator.SetBool("isHoldingWeapon", false);
        Player.Instance.Animator.SetBool("isAiming", false);
        Player.Instance.Animator.SetBool("isShooting", false);
        Player.Instance.Animator.ResetTrigger("ReloadEnd");
        Player.Instance.Animator.Play("Not Holding Weapon");
    }

    public void AimingWeaponAnimation(bool input)
    {
        if (!Player.Instance.WeaponPickup.IsholdingWeapon()) return;
        if (input && !Player.Instance.GetPlayerGun().ReloadGun.reloading) Player.Instance.Animator.SetBool("isAiming", true);
        else Player.Instance.Animator.SetBool("isAiming", false);
    }
}
