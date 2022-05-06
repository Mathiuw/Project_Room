using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject crosshair_Dot;
    [SerializeField] private GameObject crosshair_Weapon;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        CrossHairCheck();
    }

    private void CrossHairCheck()
    {
        if (!WeaponPickup.IsHoldingWeapon())
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
            return;
        }

        ShootGun gunScript = transform.root.GetComponentInChildren<ShootGun>();

        while (animator.GetBool("isAiming") || gunScript.reloading)
        {
            crosshair_Dot.SetActive(false);
            crosshair_Weapon.SetActive(false);
            return;
        }

        crosshair_Dot.SetActive(false);
        crosshair_Weapon.SetActive(true);
    }
}
