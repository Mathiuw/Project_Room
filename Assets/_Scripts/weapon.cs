using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class weapon : MonoBehaviour
{
    public bool isBeingHold { get; private set; }
    public Name weaponName { get; private set; }
    public ShootGun shootGun { get; private set; }
    public ReloadGun reloadGun { get; private set; }
    public WeaponAnimations weaponAnimations { get; private set; }
    public Rigidbody rb { get; private set; }
    public AudioSource WeaponSound { get; private set; }

    public event Action onBeingHold;

    void Awake() 
    {
        gameObject.layer = 12;

        weaponName = GetComponent<Name>();
        shootGun = GetComponent<ShootGun>();
        reloadGun= GetComponent<ReloadGun>();
        weaponAnimations= GetComponent<WeaponAnimations>();
        rb = GetComponent<Rigidbody>();
        WeaponSound = GetComponent<AudioSource>();
    }

    public void OnBeingHold(bool b) 
    {
        isBeingHold= b;

        weaponName.enabled= !b;

        rb.isKinematic= b;
        if (b) rb.interpolation = RigidbodyInterpolation.None;
        else rb.interpolation = RigidbodyInterpolation.Interpolate;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = false;

        onBeingHold?.Invoke();  
    }
}
