using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject AmmoUI;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();       
    }

    private void Update()
    {
        animator.SetFloat("RbVelocity", rb.velocity.magnitude);

        HoldWeaponAnimation();
        AimingAnimation();
    }

    private void HoldWeaponAnimation()
    {
        if (WeaponPickup.IsHoldingWeapon())
        {
            animator.SetBool("isHoldingWeapon", true);
            AmmoUI.SetActive(true);
        }
        else
        {
            animator.SetBool("isHoldingWeapon", false);
            AmmoUI.SetActive(false);
        }
    }

    private void AimingAnimation()
    {
        if (WeaponPickup.IsHoldingWeapon())
        {
            ShootGun weaponScript = transform.parent.GetComponentInChildren<ShootGun>();

            if (Input.GetKeyDown(KeyCode.Mouse1) && !weaponScript.reloading)
            {
                animator.SetBool("isAiming", true);
            }
            if (!Input.GetKey(KeyCode.Mouse1))
            {
                animator.SetBool("isAiming", false);
            }
        }
    }
}
