using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteract))]
[RequireComponent(typeof(PlayerWeaponInteraction))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    // Input class
    GameActions input;

    [SerializeField] Transform playerCameraPosition;
    PlayerCamera playerCamera;
    Rigidbody rb;

    public GameActions GetInput() { return input; }
    public Transform GetCameraPosition() { return playerCameraPosition; }
    public PlayerCamera GetPlayerCamera() { return playerCamera; }

    void OnEnable()
    {
        // Enable input
        input.Enable();
    }

    void OnDisable()
    {
        // Disable input
        input.Disable();
    }

    void Awake()
    {
        // Create input class
        input = new GameActions();

        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Bind on on dead health event
        GetComponent<Health>().onDead += OnDeadFunc;
    }

    void Start()
    {
        // Find PlayerCamera
        PlayerCamera playerCameraComponent = FindAnyObjectByType<PlayerCamera>();

        if (playerCameraComponent != null)
        {
            playerCamera = playerCameraComponent;
        }
        else 
        {
            Debug.Log("Cant find PlayerCamera");
            enabled = false;
        }
        
    }

    void OnDeadFunc()
    {
        rb.freezeRotation = false;
        input.Disable();
    }
}