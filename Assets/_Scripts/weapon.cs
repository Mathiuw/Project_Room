using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isBeingHold { get; private set; } = false;
    public bool isBeingAim { get; private set; } = false;

    public Name weaponName { get; private set; }
    public WeaponShoot shootGun { get; private set; }
    public ReloadGun reloadGun { get; private set; }
    public Rigidbody rb { get; private set; }

    public Action<PlayerWeaponInteraction, bool> onHoldStateChange;

    void Start() 
    {
        weaponName = GetComponent<Name>();
        shootGun = GetComponent<WeaponShoot>();
        reloadGun = GetComponent<ReloadGun>();
        rb = GetComponent<Rigidbody>(); 

        SetHoldState(false);
    } 

    void SetAimFalse() { isBeingAim = false; }

    void SetAimTrue() { isBeingAim = true; }

    void DropAim(Transform weapon) { isBeingAim = false; }

    public void SetHoldState(bool b, PlayerWeaponInteraction playerWeaponInteraction = null) 
    {
        isBeingHold = b;
        weaponName.enabled = !b;
        rb.isKinematic = b;

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

        SetPlayerEvents(playerWeaponInteraction, b);
        onHoldStateChange?.Invoke(playerWeaponInteraction, b);
    }

    public void SetPlayerEvents(PlayerWeaponInteraction playerWeaponInteraction, bool state) 
    {
        if (playerWeaponInteraction == null) return;

        if (state)
        {
            playerWeaponInteraction.onAimStart += SetAimTrue;
            playerWeaponInteraction.onAimEnd += SetAimFalse;
            playerWeaponInteraction.onDrop += DropAim;
        }
        else
        {
            playerWeaponInteraction.onAimStart -= SetAimTrue;
            playerWeaponInteraction.onAimEnd -= SetAimFalse;
            playerWeaponInteraction.onDrop -= DropAim;
        }
    }
}
