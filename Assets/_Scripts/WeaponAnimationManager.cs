using UnityEngine;

[RequireComponent(typeof(ShootGun))]
[RequireComponent(typeof(Animator))]
public class WeaponAnimationManager : MonoBehaviour
{
    ShootGun shootGun;
    Animator animator;
    weapon weapon;

    void Awake() 
    {
        shootGun = GetComponent<ShootGun>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<weapon>();

        weapon.onHoldStateChange += HoldStateChange;
    } 

    void Start() 
    {
        SetAnimationTime();
        shootGun.onShoot += ShootWeapon;
    }

    void ShootWeapon() 
    {
        if (!weapon.isBeingAim) animator.Play("Shoot");
        else animator.Play("Aim Shoot");
    }

    void SetAnimationTime() 
    {
        animator.SetFloat("Time", shootGun.fireRate); 
    }

    void HoldStateChange(bool b) 
    {
        animator.Play("Idle");
        animator.enabled = b;
    }
}
