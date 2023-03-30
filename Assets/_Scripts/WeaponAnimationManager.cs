using System.Collections;
using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    Animator animator;
    Weapon weapon;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponent<Weapon>();

        SetAnimationTime();
        animator.enabled = false;
    }

    public void ShootWeapon() 
    {
        if (!weapon.isBeingAim) animator.Play("Shoot", -1, 0f);
        else animator.Play("Aim Shoot", - 1, 0f);
    }

    void SetAnimationTime() 
    {
        animator.SetFloat("Time", weapon.weaponSO.firerate); 
    }
}
