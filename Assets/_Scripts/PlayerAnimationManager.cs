using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using System;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] GameObject ammoUI;
    [SerializeField] Image ammoType;

    WeaponInteraction weaponInteraction;
    Animator animator;
    Rigidbody rb;

    void Start() 
    {
        weaponInteraction = GetComponent<WeaponInteraction>();
        animator = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody>();

        OnHoldWeaponAnimation(transform);

        weaponInteraction.onPickupStart += SetWeaponAnimations;
        weaponInteraction.onPickupEnd += OnPickup;
        weaponInteraction.onPickupEnd += OnHoldWeaponAnimation;
        weaponInteraction.onPickupEnd += OnShoot;
        weaponInteraction.onWeaponDrop += OnDrop;
        weaponInteraction.onWeaponDrop += OnHoldWeaponAnimation;

    }

    void Update() => animator.SetFloat("RbVelocity", rb.velocity.magnitude);

    void ShootWeapon() => animator.SetTrigger("isShooting");

    public void OnShoot(Transform gun) => gun.GetComponent<ShootGun>().onShoot += ShootWeapon;
 
    public void AimWeapon(bool b)
    {
        if (b) animator.SetBool("isAiming", true);
        else animator.SetBool("isAiming", false);
    }

    public void SetWeaponAnimations(Transform gun) 
    {
        animator.runtimeAnimatorController = gun.GetComponent<WeaponAnimations>().WeaponOverrideController;
    }

    public void OnHoldWeaponAnimation(Transform gun)
    {
        bool isHoldingWeapon = weaponInteraction.isHoldingWeapon;

        animator.SetBool("isHoldingWeapon", isHoldingWeapon);
        ammoUI.SetActive(isHoldingWeapon);
        ammoType.gameObject.SetActive(isHoldingWeapon);
    }

    public void OnPickup(Transform gun) => animator.SetBool("isHoldingWeapon", true);

    public void OnDrop(Transform gun) 
    {
        animator.SetBool("isHoldingWeapon", false);
        animator.SetBool("isAiming", false);
        animator.SetBool("isShooting", false);
        animator.ResetTrigger("ReloadEnd");
        animator.Play("Not Holding Weapon");
    }
}
