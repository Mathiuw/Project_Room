using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    Rigidbody rb;
    Inventory inventory;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Awake()
    {
        Instance = this;

        rb = GetComponentInChildren<Rigidbody>();
        playerWeaponInteraction = GetComponentInChildren<PlayerWeaponInteraction>();
        inventory= GetComponentInChildren<Inventory>();

        playerWeaponInteraction.raycastTransform = Camera.main.transform;
        playerWeaponInteraction.onPickupEnd += OnPickupEnd;
        playerWeaponInteraction.onWeaponDrop+= OnWeaponDrop;

        GetComponentInChildren<Die>().Died += OnDead;

        Pause.instance.Paused += OnPause;

        UI_Inventory.instance.SetInventory(inventory);
    }

    void Update() 
    {
        UI_Inventory.instance.ShowAmmoInUI(playerWeaponInteraction);
    }

    public Transform PlayerTransform() => transform.Find("Player");

    public ShootGun GetPlayerGun() 
    {
       if (playerWeaponInteraction.isHoldingWeapon) return GetComponentInChildren<ShootGun>();
       Debug.LogError("Player Doesnt Have Gun");
       return null;
    }

    void OnPickupEnd(Transform gun) 
    {
        gun.GetComponentInParent<ShootGun>().onHit += UI_Hit.Instance.OnHit;
    }

    void OnWeaponDrop(Transform gun) 
    {
        gun.GetComponentInParent<ShootGun>().onHit -= UI_Hit.Instance.OnHit;
    }

    void OnPause(bool b) 
    {
        GetComponentInChildren<WeaponSway>().enabled = !b;
        GetComponentInChildren<CameraRotateSideways>().enabled = !b;
        GetComponentInChildren<PlayerMovement>().enabled = !b;
        GetComponentInChildren<CameraMove>().enabled = !b;
        GetComponentInChildren<PlayerInteract>().enabled = !b;
        GetComponentInChildren<PlayerWeaponInteraction>().enabled = !b;
    }

    void OnDead() 
    {
        playerWeaponInteraction.DropGun();
        rb.freezeRotation = false;
        transform.Find("UI").gameObject.SetActive(false);
        GetComponentInChildren<CameraRotateSideways>().enabled = false;
        GetComponentInChildren<PlayerMovement>().enabled = false;
        GetComponentInChildren<CameraMove>().enabled = false;
        GetComponentInChildren<PlayerInteract>().enabled = false;
        GetComponentInChildren<PlayerWeaponInteraction>().enabled = false;
        GetComponentInChildren<PlayerBodyRotation>().enabled= false;
        GetComponentInChildren<Die>().Died -= OnDead;
    }
}
