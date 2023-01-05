using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Transform UI { get; private set; }
    public Transform PlayerCamera { get; private set; }
    public Transform GunHolder { get; private set; }
    public Rigidbody RigidBody { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerAnimationManager PlayerAnimationManager { get; private set; }
    public WeaponInteraction WeaponInteraction { get; private set; }
    public UseAndDropItems UseAndDropItems { get; private set; }
    public Inventory Inventory { get; private set; }
    public UI_Inventory UIInventory { get; private set; }
    public SelectItem selectItem { get; private set; }
    public Health Health { get; private set; }
    public Die die { get; private set; }
    public Sprint Sprint { get; private set; }
    public Movement Movement { get; private set; }

    void Awake()
    {
        Instance = this;

        UI = transform.transform.Find("UI").transform;
        PlayerCamera = Camera.main.transform;
        GunHolder = transform.Find("Gun_holder");

        RigidBody = GetComponentInChildren<Rigidbody>();
        Animator = GetComponent<Animator>();
        PlayerAnimationManager = GetComponentInChildren<PlayerAnimationManager>();
        WeaponInteraction = GetComponentInChildren<WeaponInteraction>();
        UseAndDropItems = GetComponentInChildren<UseAndDropItems>();
        Inventory = GetComponentInChildren<Inventory>();
        UIInventory = GetComponentInChildren<UI_Inventory>();
        selectItem = GetComponentInChildren<SelectItem>();
        Health = GetComponentInChildren<Health>();
        die= GetComponentInChildren<Die>();
        Sprint = GetComponentInChildren<Sprint>();
        Movement = GetComponentInChildren<Movement>();

        WeaponInteraction.raycastTransform = Camera.main.transform;
        WeaponInteraction.onPickupCoroutineEnd += UI_Hit.Instance.OnPickupWeapon;

        die.Died += OnDisablePlayerFreezeRotation;
        die.Died += WeaponInteraction.DropGun;
        die.Died += OnDisableUI;  

        Pause.instance.Paused += GetComponent<PlayerInput>().OnEnableDisable;
    }

    public Transform PlayerTransform() => transform.Find("Player");

    public ShootGun GetPlayerGun() 
    {
       if (WeaponInteraction.IsholdingWeapon()) return GetComponentInChildren<ShootGun>();
       Debug.LogError("Player Doesnt Have Gun");
       return null;
    }

    void OnDisablePlayerFreezeRotation() => RigidBody.freezeRotation = false;

    void OnDisableUI() => UI.gameObject.SetActive(false);
}
