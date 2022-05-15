using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour,ICanDo
{
    [SerializeField]private bool canDo = true;

    private Animator animator;

    [SerializeField] private Transform gunHolder;

    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    private void Awake()
    {
        animator = GameObject.Find("Player_And_Camera").GetComponent<Animator>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    private void Update()
    {
        if (!canDo) return;

        if (WeaponPickup.IsHoldingWeapon)
        {
            Sway();

            if (animator.GetBool("isAiming") == true) swayMultiplier = 0.2f;
            else swayMultiplier = 2f;
        }
    }

    private void Sway()
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

    public void CheckIfCanDo(bool check)
    {
        if (check)
        {
            canDo = false;
        }
        else canDo = true;
    }
}
