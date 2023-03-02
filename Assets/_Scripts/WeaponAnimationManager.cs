using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    ShootGun shootGun;
    Animator animator;
    weapon weapon;

    public void SetAttributes(ShootGun shootGun, Animator animator, weapon weapon) 
    {
        this.shootGun= shootGun;
        this.animator = animator;
        this.weapon = weapon;

        SetAnimationTime();
        weapon.onHoldStateChange += HoldStateChange;
        shootGun.onShoot += ShootWeapon;
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

    void HoldStateChange(bool b) 
    {
        animator.Play("Idle");
        animator.enabled = b;
    }
}
