using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] Transform cameraPosition;
    public Transform CameraPosition { get => cameraPosition; set => cameraPosition = value; }

    Rigidbody rb;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerWeaponInteraction = GetComponent<PlayerWeaponInteraction>();

        GetComponent<Health>().onDead += OnDead;
    }

    void Start() 
    {
        if (Pause.instance != null) Pause.instance.onPause += OnPause;
        else Debug.LogError("Cant Find Player UI");
    }

    void OnPause(bool b) 
    {
        GetComponentInChildren<PlayerMovement>().enabled = !b;
        GetComponentInChildren<PlayerInteract>().enabled = !b;
        GetComponentInChildren<PlayerWeaponInteraction>().enabled = !b;
    }

    void OnDead() 
    {
        playerWeaponInteraction.DropWeapon();
        rb.freezeRotation = false;
        GetComponentInChildren<PlayerMovement>().enabled = false;
        GetComponentInChildren<PlayerInteract>().enabled = false;
        GetComponentInChildren<PlayerWeaponInteraction>().enabled = false;
    }
}
