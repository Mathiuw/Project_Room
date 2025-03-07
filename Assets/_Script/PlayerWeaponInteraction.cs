﻿using System;
using System.Collections;
using UnityEngine;

public class PlayerWeaponInteraction : WeaponInteraction
{
    [Header("Weapon Interation")]
    [SerializeField] float aimTime;

    [Header("Weapon Sway")]
    [SerializeField] float smooth = 8;
    [SerializeField] float swayMultiplier = 4;

    bool isAiming = false;
    bool isReloading = false;
    bool isLerping = false;

    Transform cameraTransform;
    Inventory inventory;
    PlayerMovement playerMovement;

    // Set hold position for whem mot aiming
    Vector3 defautHoldPosition;

    public event Action onWeaponShot;
    public event Action onAimStart;
    public event Action onAimEnd;
    public event Action<Weapon> onWeaponPickup;
    public event Action<float> onReloadStart;
    public event Action onReloadEnd;
    public event Action onWeaponDrop;

    public bool GetIsAiming() { return isAiming; }
    public bool GetIsReloading() { return isReloading; }
    public bool GetIsLerping() { return isLerping; }

    public void SetIsAiming(bool isAiming, Vector3 aimVector)
    {
        this.isAiming = isAiming;

        StopAllCoroutines();
        StartCoroutine(LerpWeaponCoroutine(aimTime, weapon.transform, aimVector, Quaternion.identity));
    }

    void Awake() 
    {
        inventory = GetComponent<Inventory>();
        playerMovement = GetComponent<PlayerMovement>();
    } 

    void Start() 
    {
        cameraTransform = Camera.main.transform;

        CameraMovement playerCamera = FindAnyObjectByType<CameraMovement>();

        if (playerCamera) 
        {
            weaponHolder = playerCamera.GetWeaponHolder();

            // set holder position for aimming
            defautHoldPosition = weaponHolder.transform.localPosition;
        } 
    } 

    void Update()
    {
        Shoot();

        if (Input.GetKeyDown(KeyCode.Mouse1)) EnableAim();
        else if(Input.GetKeyUp(KeyCode.Mouse1)) StopAim();

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

        if (Input.GetKeyDown(KeyCode.G)) DropWeapon();

        // Weapon sway
        if (isHoldingWeapon) 
        {
            if (isAiming) SwayWeapon(swayMultiplier / 20);
            else SwayWeapon(swayMultiplier);
        }
    }

    IEnumerator LerpWeaponCoroutine(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation, Transform parent = null) 
    {
        isLerping = true;

        if (parent != null)
        {
            weapon.SetParent(parent);
            weapon.localScale = Vector3.one;
        }

        float elapsedTime = 0f;
        float percentageComplete = 0f;

        Vector3 startPosition = weapon.localPosition;
        Quaternion startRotation = weapon.localRotation;

        while (elapsedTime < time)
        {
            weapon.localPosition = Vector3.Lerp(startPosition, desiredPosition, percentageComplete);
            weapon.localRotation = Quaternion.Lerp(startRotation, desiredRotation, percentageComplete);

            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / time;
            yield return null;
        }

        weapon.localPosition = desiredPosition;
        weapon.localRotation = desiredRotation;
        isLerping = false;
    }

    void SwayWeapon(float swayMultiplier)
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        weapon.transform.localRotation = Quaternion.Slerp(weapon.transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    public override IEnumerator PickUpWeapon(Weapon pickedWeapon)
    {
        if (isHoldingWeapon) yield break;
        if (isLerping) yield break;
        if (pickedWeapon.holder != null)
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        //Set current weapon
        weapon = pickedWeapon;

        //Set weapon hold state
        weapon.SetHoldState(true, transform);
        
        //Lerp weapon to player
        StartCoroutine(LerpWeaponCoroutine(0.2f, pickedWeapon.transform, Vector3.zero, Quaternion.identity, weaponHolder));
        while (isLerping) yield return null;

        isHoldingWeapon = true;
        //SetWeaponEvents(true);
        onWeaponPickup?.Invoke(base.weapon);

        Debug.Log("Picked up weapon");
    }

    bool InputShoot() 
    {
        switch (weapon.GetSOWeapon().shootType)
        {
            case SOWeapon.ShootType.Single: 
                return Input.GetKeyDown(KeyCode.Mouse0);
            case SOWeapon.ShootType.Automatic:
                return Input.GetKey(KeyCode.Mouse0);
            default: 
                return false;
        }
    }

    void Shoot()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;
        if (isLerping) return;

        if (InputShoot()) 
        {
            weapon.Shoot(cameraTransform);
            onWeaponShot?.Invoke();
        } ;
    }

    void EnableAim()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        Vector3 aimVector = -defautHoldPosition - weapon.GetAimLocation();

        if (playerMovement != null)
        {
            playerMovement.SetCanSprint(false);
        }

        SetIsAiming(true, aimVector);
        onAimStart?.Invoke();
    }

    void StopAim() 
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        if (playerMovement != null)
        {
            playerMovement.SetCanSprint(true);
        }

        SetIsAiming(false, Vector3.zero);
        onAimEnd?.Invoke();
    }

    public override IEnumerator ReloadWeapon()  
    {
        if (!isHoldingWeapon) yield break;
        if (isReloading) yield break;
        if (isAiming) yield break;
        if(weapon.GetAmmo() == weapon.GetMaxAmmo()) yield break;
        if (inventory == null) yield break;
        if (!inventory.HaveItem(weapon.GetReloadItem())) yield break;

        isReloading = true;
        inventory.RemoveItem(weapon.GetReloadItem());
        onReloadStart?.Invoke(weapon.GetMaxAmmo());

        yield return new WaitForSeconds(weapon.GetReloadTime());

        weapon.AddAmmo(weapon.GetMaxAmmo());
        isReloading = false;
        onReloadEnd?.Invoke();

        yield break;
    }

    public override void DropWeapon()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        StopAllCoroutines();
        onWeaponDrop?.Invoke();

        Transform weaponTransform = weapon.transform;
        Rigidbody weaponRb = weaponTransform.GetComponent<Rigidbody>();

        weapon.SetHoldState(false, null);
        weaponTransform.SetParent(null);
        weaponTransform.transform.position = transform.position;
        weaponRb.AddForce(transform.forward * 5, ForceMode.VelocityChange);
        weaponTransform.localScale = Vector3.one;

        isLerping = false;
        isAiming = false;
        isReloading = false;
        isHoldingWeapon = false;

        weapon = null;

        Debug.Log("Dropped weapon");
    }
}
