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

        // Adiciona a animação da arma ao evento de atirar
        weapon.onShoot += ShootWeaponAnimation;

        SetAnimationTime();
        animator.enabled = true;
    }

    // Deixa o tempo da animação de acordo com o firerate da arma
    void SetAnimationTime()
    {
        animator.SetFloat("Time", weapon.GetFirerate());
    }

    public void ShootWeaponAnimation() 
    {
        PlayerWeaponInteraction playerWeaponInteraction = weapon.holder.GetComponent<PlayerWeaponInteraction>();

        if (playerWeaponInteraction != null) 
        {

            if (!playerWeaponInteraction.GetIsAiming()) animator.Play("Shoot", -1, 0f);
            else animator.Play("Aim Shoot", -1, 0f);
        }
    }
}
