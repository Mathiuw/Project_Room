using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    WeaponShoot shootGun;
    Animator animator;
    Weapon weapon;

    public void SetAttributes(WeaponShoot shootGun, Animator animator, Weapon weapon) 
    {
        this.shootGun= shootGun;
        this.animator = animator;
        this.weapon = weapon;

        SetAnimationTime();
        weapon.onHoldStateChange += HoldStateChange;
    }

    void ShootWeapon() 
    {
        if (!weapon.isBeingAim) animator.Play("Shoot");
        else animator.Play("Aim Shoot");
    }

    void SetAnimationTime() 
    {
        animator.SetFloat("Time", shootGun.firerate); 
    }

    void HoldStateChange(PlayerWeaponInteraction playerWeaponInteraction, bool b) 
    {
        animator.enabled = b;

        if (playerWeaponInteraction == null) return;
        
        if (b) shootGun.onShoot += ShootWeapon;
        else shootGun.onShoot -= ShootWeapon;

        animator.Play("Idle");
    }
}
