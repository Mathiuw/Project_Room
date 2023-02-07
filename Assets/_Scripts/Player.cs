using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }

    Rigidbody rb;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Awake()
    {
        instance = this;

        rb = GetComponentInChildren<Rigidbody>();
        playerWeaponInteraction = GetComponentInChildren<PlayerWeaponInteraction>();
    }

    void Start() 
    {
        playerWeaponInteraction.onPickupEnd += OnPickupEnd;
        playerWeaponInteraction.onWeaponDrop += OnWeaponDrop;

        GetComponentInChildren<Die>().onDead += OnDead;

        Pause.instance.Paused += OnPause;
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
        GetComponentInChildren<PlayerCameraMove>().enabled = !b;
        GetComponentInChildren<PlayerInteract>().enabled = !b;
        GetComponentInChildren<PlayerWeaponInteraction>().enabled = !b;
    }

    void OnDead() 
    {
        playerWeaponInteraction.DropGun();
        rb.freezeRotation = false;
        transform.Find("PlayerUI").gameObject.SetActive(false);
        GetComponentInChildren<CameraRotateSideways>().enabled = false;
        GetComponentInChildren<PlayerMovement>().enabled = false;
        GetComponentInChildren<PlayerCameraMove>().enabled = false;
        GetComponentInChildren<PlayerInteract>().enabled = false;
        GetComponentInChildren<PlayerWeaponInteraction>().enabled = false;
        GetComponentInChildren<PlayerBodyRotation>().enabled= false;
        GetComponentInChildren<Die>().onDead -= OnDead;
    }
}
