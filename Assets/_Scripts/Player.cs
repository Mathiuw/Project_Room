using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerWeaponInteraction))]
[RequireComponent(typeof(Inventory))]
public class Player : MonoBehaviour
{
    // Input class
    GameActions input;

    [SerializeField] Transform playerCameraDesiredPosition;
    PlayerCamera playerCamera;
    Rigidbody rb;

    [Header("Interact")]
    [SerializeField] LayerMask interactiveMask;
    [SerializeField] float rayLength = 5;

    public GameActions GetInput() { return input; }
    public Transform GetCameraDesiredPosition() { return playerCameraDesiredPosition; }
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
        PlayerCamera playerCameraComponent = FindFirstObjectByType<PlayerCamera>();

        if (playerCameraComponent != null)
        {
            playerCamera = playerCameraComponent;
        }
        else 
        {
            Debug.Log("Cant find PlayerCamera");
            enabled = false;
        }

        // Add movement input to player GameActions class
        input.Player.Interact.started += Interact;

    }

    public void Interact(InputAction.CallbackContext value)
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayLength, interactiveMask))
        {
            IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(transform);
            }
        }
    }

    void OnDeadFunc()
    {
        rb.freezeRotation = false;
        input.Disable();
    }
}