using System.Collections;
using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    Animator animator;
    Weapon weapon;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponent<Weapon>();

        SetAnimationTime();
        animator.enabled = false;
    }

    void SetAnimationTime()
    {
        animator.SetFloat("Time", weapon.GetFirerate());
    }

    public void ShootWeaponAnimation() 
    {
        PlayerWeaponInteraction playerWeaponInteraction = weapon.holder.GetComponent<PlayerWeaponInteraction>();

        if (playerWeaponInteraction != null) 
        {

            if (!playerWeaponInteraction.isAiming) animator.Play("Shoot", -1, 0f);
            else animator.Play("Aim Shoot", -1, 0f);
        }
    }
}
