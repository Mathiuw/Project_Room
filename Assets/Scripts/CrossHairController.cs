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
        if (WeaponPickup.IsHoldingWeapon())
        {
            if (animator.GetBool("isAiming") == true)
            {
                crosshair_Dot.SetActive(false);
                crosshair_Weapon.SetActive(false);
            }
            else
            {
                crosshair_Dot.SetActive(false);
                crosshair_Weapon.SetActive(true);
            }
        }
        else
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
        }
    }
}
