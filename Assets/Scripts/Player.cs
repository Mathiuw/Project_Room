using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Header("Grounded Check")]
    [SerializeField] Transform checkSphereLocation;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float sphereRadius;

    public static Player Instance { get; private set; }
    public Transform UI { get; private set; }
    public Transform PlayerCamera { get; private set; }
    public Transform GunHolder { get; private set; }
    public Rigidbody RigidBody { get; private set; }
    public Animator Animator { get; private set; }
    public AnimationStateController AnimationStateController { get; private set; }
    public WeaponPickup WeaponPickup { get; private set; }
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
        PlayerCamera = GameObject.Find("Main Camera").transform;
        GunHolder = transform.Find("Gun_holder");

        RigidBody = GetComponentInChildren<Rigidbody>();
        Animator = GetComponent<Animator>();
        AnimationStateController = GetComponentInChildren<AnimationStateController>(); 
        WeaponPickup = GetComponentInChildren<WeaponPickup>();
        UseAndDropItems = GetComponentInChildren<UseAndDropItems>();
        Inventory = GetComponentInChildren<Inventory>();
        UIInventory = GetComponentInChildren<UI_Inventory>();
        selectItem = GetComponentInChildren<SelectItem>();
        Health = GetComponentInChildren<Health>();
        die= GetComponentInChildren<Die>();
        Sprint = GetComponentInChildren<Sprint>();
        Movement = GetComponentInChildren<Movement>();

        die.OnDieAction += DisableFreezeRotation;
        die.OnDieAction += WeaponPickup.DropGun;
        die.OnDieAction += DisableUI;
    }

    public Transform PlayerTransform() => transform.Find("Player");

    public ShootGun GetPlayerGun() 
    {
       if (WeaponPickup.IsholdingWeapon()) return GetComponentInChildren<ShootGun>();
       Debug.LogError("Player Doesnt Have Gun");
       return null;
    }

    void DisableFreezeRotation() => RigidBody.freezeRotation = false;

    void DisableUI() => UI.gameObject.SetActive(false);

    public bool IsGrounded() => Physics.CheckSphere(checkSphereLocation.position, sphereRadius, groundMask);

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(checkSphereLocation.position, sphereRadius);
    }
}
