using UnityEngine;

public class WeaponAnimationManager : MonoBehaviour
{
    Animator animator;
    Weapon weapon;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.enabled = true;
    }

    private void OnEnable()
    {
        weapon = GetComponent<Weapon>();
        weapon.onShoot += ShootWeaponAnimation;

        SetAnimationTime();
    }

    private void OnDisable()
    {
        weapon.onShoot -= ShootWeaponAnimation;
    }

    // Deixa o tempo da animação de acordo com o firerate da arma
    void SetAnimationTime()
    {
        animator.SetFloat("Time", weapon.GetFirerate());
    }

    public void ShootWeaponAnimation() 
    {
        PlayerWeaponInteraction playerWeaponInteraction = weapon.owner.GetComponent<PlayerWeaponInteraction>();

        if (playerWeaponInteraction != null) 
        {

            if (!playerWeaponInteraction.GetIsAiming()) animator.Play("Shoot", -1, 0f);
            else animator.Play("Aim Shoot", -1, 0f);
        }
    }
}
