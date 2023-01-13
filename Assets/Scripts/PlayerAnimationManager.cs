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
        weaponInteraction = Player.Instance.WeaponInteraction;
        animator = Player.Instance.Animator;
        rb = Player.Instance.RigidBody;

        OnHoldWeaponAnimation(transform);

        weaponInteraction.PickupStarted += OnSetWeaponAnimations;
        weaponInteraction.PickupEnded += OnPickupWeaponAnimation;
        weaponInteraction.PickupEnded += OnHoldWeaponAnimation;
        weaponInteraction.weaponDropped += OnDropWeaponAnimationReset;
        weaponInteraction.weaponDropped += OnHoldWeaponAnimation;
        weaponInteraction.PickupEnded += OnWeaponShootSetTrigger;
    }

    void Update() => animator.SetFloat("RbVelocity", rb.velocity.magnitude);

    //Ativa a Animação de Atirar da Arma
    void ShootWeaponAnimation() => animator.SetTrigger("isShooting");

    //Adiciona o trigger da animação ao Evento de Atirar da arma
    public void OnWeaponShootSetTrigger(Transform gun) => gun.GetComponent<ShootGun>().onShoot += ShootWeaponAnimation;
 
    //Ativa a Animação de Mirar a Arma
    public void AimingWeaponAnimation(bool input)
    {
        if (!Player.Instance.WeaponInteraction.isHoldingWeapon) return;
        if (input && !Player.Instance.GetPlayerGun().ReloadGun.reloading) Player.Instance.Animator.SetBool("isAiming", true);
        else Player.Instance.Animator.SetBool("isAiming", false);
    }

    //Define as Animações da Arma
    public void OnSetWeaponAnimations(Transform gun) 
    {
        animator.runtimeAnimatorController = gun.GetComponent<WeaponAnimations>().WeaponOverrideController;
    }

    //Ativa ou Desativa a Animação de Segurar da Arma
    public void OnHoldWeaponAnimation(Transform gun)
    {
        bool isHoldingWeapon = weaponInteraction.isHoldingWeapon;

        animator.SetBool("isHoldingWeapon", isHoldingWeapon);
        ammoUI.SetActive(isHoldingWeapon);
        ammoType.gameObject.SetActive(isHoldingWeapon);
    }

    //Ativa a Animação de Pegar a Arma
    public void OnPickupWeaponAnimation(Transform gun) => animator.SetBool("isHoldingWeapon", true);

    //Desativa as Animações ao Dropar a Arma
    public void OnDropWeaponAnimationReset(Transform gun) 
    {
        animator.SetBool("isHoldingWeapon", false);
        animator.SetBool("isAiming", false);
        animator.SetBool("isShooting", false);
        animator.ResetTrigger("ReloadEnd");
        animator.Play("Not Holding Weapon");
    }
}
