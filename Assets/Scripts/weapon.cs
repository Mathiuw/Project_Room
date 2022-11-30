using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [Header("Weapon Status")]
    [SerializeField] private bool isBeingHold = false;
    public bool IsBeingHold { get => isBeingHold; set => isBeingHold = value; }
    public Name weaponName { get; private set; }
    public ShootGun shootGun { get; private set; }
    public ReloadGun reloadGun { get; private set; }
    public WeaponAnimations weaponAnimations { get; private set; }
    public Rigidbody rb { get; private set; }
    
    void Awake() 
    {
        weaponName = GetComponent<Name>();
        shootGun = GetComponent<ShootGun>();
        reloadGun= GetComponent<ReloadGun>();
        weaponAnimations= GetComponent<WeaponAnimations>();
        rb = GetComponent<Rigidbody>();
    }

    public void BeingHold(bool b) => IsBeingHold = b;
}
