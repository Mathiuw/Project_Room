using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public Rigidbody RigidBody { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerAnimationManager PlayerAnimationManager { get; private set; }
    public WeaponInteraction WeaponInteraction { get; private set; }
    public UseAndDropItems UseAndDropItems { get; private set; }
    public Inventory Inventory { get; private set; }
    public PlayerInteract PlayerInteract { get; private set; }
    public UI_Inventory UIInventory { get; private set; }
    public UI_SelectItem selectItem { get; private set; }
    public Health Health { get; private set; }
    public Sprint Sprint { get; private set; }
    public Movement Movement { get; private set; }

    void Awake()
    {
        Instance = this;

        playerInput= GetComponent<PlayerInput>();
        RigidBody = GetComponentInChildren<Rigidbody>();
        Animator = GetComponent<Animator>();
        PlayerAnimationManager = GetComponentInChildren<PlayerAnimationManager>();
        WeaponInteraction = GetComponentInChildren<WeaponInteraction>();
        UseAndDropItems = GetComponentInChildren<UseAndDropItems>();
        Inventory = GetComponentInChildren<Inventory>();
        PlayerInteract = GetComponentInChildren<PlayerInteract>();
        UIInventory = GetComponentInChildren<UI_Inventory>();
        selectItem = GetComponentInChildren<UI_SelectItem>();
        Health = GetComponentInChildren<Health>();
        Sprint = GetComponentInChildren<Sprint>();
        Movement = GetComponentInChildren<Movement>();

        WeaponInteraction.raycastTransform = Camera.main.transform;
        WeaponInteraction.onPickupEnd += OnPickupEnd;
        WeaponInteraction.onWeaponDrop+= OnWeaponDrop;

        GetComponentInChildren<Die>().Died += OnDead;

        Pause.instance.Paused += OnPause;
    }

    public Transform PlayerTransform() => transform.Find("Player");

    public ShootGun GetPlayerGun() 
    {
       if (WeaponInteraction.isHoldingWeapon) return GetComponentInChildren<ShootGun>();
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
        GetComponent<PlayerInput>().enabled = !b;
        GetComponentInChildren<WeaponSway>().enabled = !b;
        GetComponentInChildren<CameraRotateSideways>().enabled = !b;
    }

    void OnDead() 
    {
        playerInput.enabled=false;
        WeaponInteraction.DropGun();
        RigidBody.freezeRotation = false;
        transform.Find("UI").gameObject.SetActive(false);
        GetComponentInChildren<CameraRotateSideways>().enabled = false;
        GetComponentInChildren<Die>().Died -= OnDead;
    }
}
