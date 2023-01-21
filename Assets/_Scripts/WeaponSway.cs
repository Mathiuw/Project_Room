using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;
    PlayerWeaponInteraction playerWeaponInteraction;
    Animator animator;

    void Awake() 
    {
        playerWeaponInteraction= GetComponent<PlayerWeaponInteraction>();
        animator = GetComponentInParent<Animator>();
    } 

    void Update()
    {
        Sway();
    }

    void Sway()
    {
        if (!playerWeaponInteraction.isHoldingWeapon) return;

        if (animator.GetBool("isAiming") == true) swayMultiplier = 0.2f;
        else swayMultiplier = 2f;

        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        gunHolder.localRotation = Quaternion.Slerp(gunHolder.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
