using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    // Input class
    GameActions input;

    [Header("Movement")]
    [SerializeField] Transform cameraPivot;
    [SerializeField] float moveSpeed = 200.0f;
    Vector2 moveVector;
    Rigidbody rb;
    Transform playerCamera;

    [Header("Sprint")]
    [SerializeField] bool canSprint = true;
    [field: SerializeField] public float MaxStamina { get; set; } = 30;
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float sprintingMultiplier = 1.5f;
    float currentSprintMultiplier = 1;

    public float Stamina { get; set; } = 0;

    public bool IsSprinting { get; set; } = false;

    // Stamina update event
    public event Action<float> staminaUpdated;

    public GameActions GetInput() { return input; }

    public Transform GetCameraPivot() { return cameraPivot; }

    void Awake()
    {
        // Create input class
        input = new GameActions();

        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCanceled;

        input.Enable();

        rb = GetComponent<Rigidbody>();

        TryGetComponent(out Health health);

        if (health)
        {
            health.onDead += OnDied;
        }

        Stamina = MaxStamina;
    }

    void OnDisable()
    {
        input.Player.Move.performed -= OnMovementPerformed;
        input.Player.Move.canceled -= OnMovementCanceled;

        input.Disable();
    }

    void Start()
    {
        playerCamera = FindAnyObjectByType<Camera>().transform;

        // Update stamina event call
        staminaUpdated?.Invoke(Stamina);
    }

    void Update()
    {
        Sprint(KeyCode.LeftShift);
    }

    void FixedUpdate() 
    {
        Movement(moveVector.y, moveVector.x);

        // Rotate body
        transform.localRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
    }

    void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    public void Movement(float moveV, float moveH)
    {
        Vector3 moveDirection;

        moveDirection = transform.forward * moveV + transform.right * moveH;
        rb.AddForce(moveDirection.normalized * moveSpeed * currentSprintMultiplier * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Sprint(KeyCode RunInput)
    {
        if (!canSprint) return;

        if (Stamina > 0 && moveVector.y > 0 && Input.GetKey(RunInput))
        {
            IsSprinting = true;

            currentSprintMultiplier = sprintingMultiplier;

            Stamina = Stamina - (staminaCost * Time.deltaTime);
        }
        else 
        {
            IsSprinting = false;

            currentSprintMultiplier = 1f;
        }
        
        if (!Input.GetKey(RunInput) && !IsSprinting)
        {
            Stamina = Stamina + (staminaRecover * Time.deltaTime);
        }

        // Clamp stamina value
        Stamina = Math.Clamp(Stamina, 0, MaxStamina);

        staminaUpdated?.Invoke(Stamina);
    }

    void OnDied()
    {
        rb.freezeRotation = false;
        input.Disable();
    }
}