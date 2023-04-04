using UnityEngine;

public class Weapon : MonoBehaviour
{
    public SOWeapon weaponSO { get; private set; }

    public bool isBeingHold { get; private set; } = false;
    public bool isBeingAim { get; private set; } = false;

    public Transform holder { get; private set; }

    void Start() 
    {
        SetHoldState(false, null);
    }

    public void SetWeaponSO(SOWeapon weaponSO) => this.weaponSO = weaponSO;

    public void SetAimFalse() { isBeingAim = false; }

    public void SetAimTrue() { isBeingAim = true; }

    public void DropAim(Transform weapon) { isBeingAim = false; }

    public void SetHoldState(bool b, Transform holder) 
    {
        Name weaponName = GetComponent<Name>();
        Rigidbody rb = GetComponent<Rigidbody>();

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
