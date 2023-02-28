using System;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [SerializeField] Vector3 aimVector;

    public bool isBeingHold { get; private set; } = false;
    public bool isBeingAim { get; private set; } = false;

    public Name weaponName { get; private set; }
    public ShootGun shootGun { get; private set; }
    public ReloadGun reloadGun { get; private set; }
    public Rigidbody rb { get; private set; }
    public AudioSource WeaponSound { get; private set; }
    public Animator animator { get; private set; }

    public Action<bool> onHoldStateChange;

    void Awake() 
    {      
        weaponName = GetComponent<Name>();
        shootGun = GetComponent<ShootGun>();
        reloadGun= GetComponent<ReloadGun>();
        rb = GetComponent<Rigidbody>();
        WeaponSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Start() => SetHoldState(false);

    public Vector3 GetAimVector() { return aimVector; }

    public void SetAim() { isBeingAim = !isBeingAim; }

    public void SetHoldState(bool b) 
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
            if(b) renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            else renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        onHoldStateChange?.Invoke(b);
    }
}
