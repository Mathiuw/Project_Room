using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;
    Animator animator;

    void Start() => animator = Player.Instance.GetComponent<Animator>();

    void Update()
    {
        if (!Player.Instance.WeaponInteraction.isHoldingWeapon) return;

        if (animator.GetBool("isAiming") == true) swayMultiplier = 0.2f;
        else swayMultiplier = 2f;

        Sway();
    }

    void Sway()
    {
        //Mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        //Calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        //Rotate
        gunHolder.localRotation = Quaternion.Slerp(gunHolder.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
