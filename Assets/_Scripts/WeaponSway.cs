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
        if (!playerWeaponInteraction.isHoldingWeapon) return;

        Transform weapon = playerWeaponInteraction.currentWeapon.transform;

        if (playerWeaponInteraction.isAiming) Sway( weapon, swayMultiplier / 20);
        else Sway(weapon, swayMultiplier);
    } 

    void Sway(Transform weapon, float swayMultiplier)
    {       
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    void OnPause(bool b) => enabled = !b;
}
