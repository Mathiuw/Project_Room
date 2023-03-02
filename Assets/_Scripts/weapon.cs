using System;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public bool isBeingHold { get; private set; } = false;
    public bool isBeingAim { get; private set; } = false;

    public Name weaponName { get; private set; }
    public ShootGun shootGun { get; private set; }
    public ReloadGun reloadGun { get; private set; }
    public Rigidbody rb { get; private set; }

    public Action<bool> onHoldStateChange;

    void Start() 
    {
        weaponName = GetComponent<Name>();
        shootGun = GetComponent<ShootGun>();
        reloadGun = GetComponent<ReloadGun>();
        rb = GetComponent<Rigidbody>(); 

        SetHoldState(false);
    } 

    void SetAimFalse() { isBeingAim = false; }

    public void SetAimTrue() { isBeingAim = true; }

    public void SetHoldState(bool b, PlayerWeaponInteraction playerWeaponInteraction = null) 
    {
        isBeingHold = b;
        weaponName.enabled = !b;
        rb.isKinematic = b;

        if (playerWeaponInteraction != null) 
        {
            if (b)
            {
                playerWeaponInteraction.onAimStart += SetAimTrue;
                playerWeaponInteraction.onAimEnd += SetAimFalse;
                playerWeaponInteraction.onDrop += SetAimFalse;
            }
            else 
            {
                playerWeaponInteraction.onAimStart -= SetAimTrue;
                playerWeaponInteraction.onAimEnd -= SetAimFalse;
                playerWeaponInteraction.onDrop -= SetAimFalse;
            }
        }

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

        onHoldStateChange?.Invoke(b);
    }
}
