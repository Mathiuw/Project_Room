using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public SOWeapon weaponSO { get; private set; }

    public bool isBeingHold { get; private set; } = false;
    public bool isBeingAim { get; private set; } = false;

    public Transform holder { get; private set; }
    public Name weaponName { get; private set; }
    public WeaponShoot shootGun { get; private set; }
    public WeaponReload reloadGun { get; private set; }
    public Rigidbody rb { get; private set; }

    void Start() 
    {
        weaponName = GetComponent<Name>();
        shootGun = GetComponent<WeaponShoot>();
        reloadGun = GetComponent<WeaponReload>();
        rb = GetComponent<Rigidbody>(); 

        SetHoldState(false, null);
    }

    public void SetWeaponSO(SOWeapon weaponSO) => this.weaponSO = weaponSO; 

    public void SetAimFalse() { isBeingAim = false; }

    public void SetAimTrue() { isBeingAim = true; }

    public void DropAim(Transform weapon) { isBeingAim = false; }

    public void SetHoldState(bool b, Transform holder) 
    {
        isBeingHold = b;
        weaponName.enabled = !b;
        rb.isKinematic = b;

        this.holder = holder;

        if (b) rb.interpolation = RigidbodyInterpolation.None;
        else rb.interpolation = RigidbodyInterpolation.Interpolate;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = b;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++) 
        {
            if (b) renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            else renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
