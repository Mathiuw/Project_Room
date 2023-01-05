using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class weapon : MonoBehaviour
{
    [Header("Weapon State")]
    [SerializeField] private bool isBeingHold = false;

    public bool IsBeingHold { get => isBeingHold; set => isBeingHold = value; }
    public Name weaponName { get; private set; }
    public ShootGun shootGun { get; private set; }
    public ReloadGun reloadGun { get; private set; }
    public WeaponAnimations weaponAnimations { get; private set; }
    public Rigidbody rb { get; private set; }
    public AudioSource WeaponSound { get; private set; }
    
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

    public void BeingHold(bool b) => IsBeingHold = b;

    public void SetAudioSpacialBlend(float f) => WeaponSound.spatialBlend = f;
}
