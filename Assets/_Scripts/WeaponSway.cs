using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Start() 
    {
        if (Player.instance != null) playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
        else enabled = false;

        if (Pause.instance != null) Pause.instance.onPause += OnPause;
        else enabled = false;
    }

    void Update() 
    {
        if (playerWeaponInteraction.isAiming) Sway(swayMultiplier / 20);
        else Sway(swayMultiplier);
    } 

    void Sway(float swayMultiplier)
    {
        if (!playerWeaponInteraction.isHoldingWeapon) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        gunHolder.localRotation = Quaternion.Slerp(gunHolder.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    void OnPause(bool b) => enabled = !b;
}
