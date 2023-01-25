using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInteraction : WeaponInteraction
{
    PlayerAnimationManager playerAnimationManager;
    Transform mainCamera;
    Animator animator;
    Inventory inventory;

    void Awake() 
    {
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        animator = GetComponentInParent<Animator>();
        inventory = GetComponent<Inventory>();
    }

    void Start() 
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && isHoldingWeapon) currentWeapon.shootGun.Shooting(mainCamera);

        if (Input.GetKey(KeyCode.Mouse1) && isHoldingWeapon && !currentWeapon.reloadGun.reloading) playerAnimationManager.AimWeapon(true);
        else playerAnimationManager.AimWeapon(false);

        if (Input.GetKeyDown(KeyCode.R) && isHoldingWeapon) currentWeapon.reloadGun.Reloading(inventory, animator);

        if (Input.GetKeyDown(KeyCode.E)) TryToPickupWeapon();

        if (Input.GetKeyDown(KeyCode.G)) DropGun();
    }
}
