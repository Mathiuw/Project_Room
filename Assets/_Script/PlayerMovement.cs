using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] float maxStamina = 30;
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float multiplier = 1.5f;
    float stamina;
    float sprintMultiplier = 1;
    bool isSprinting = false;

    // Stamina update event
    public event Action<float> staminaUpdated;

    public GameActions GetInput() { return input; }

    public Transform GetCameraPivot() { return cameraPivot; }

    public float GetMoveSpeed() { return moveSpeed; }

    public float GetSprintMultiplier() { return sprintMultiplier; }

    public void SetSprintMultiplier(float sprintMultiplier) { this.sprintMultiplier = sprintMultiplier; }

    public float GetMaxStamina() { return maxStamina; }

    public void SetMaxStamina(float maxStamina) { this.maxStamina = maxStamina; }

    public float GetStamina() { return stamina; }

    public void SetStamina(float stamina)
    {
        this.stamina = stamina;
        this.stamina = Math.Clamp(stamina, 0, maxStamina);
        staminaUpdated?.Invoke(this.stamina);
    }

    public bool GetIsSprinting() { return isSprinting; }

    void SetIsSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;

        if (isSprinting) SetSprintMultiplier(multiplier);
        else SetSprintMultiplier(1);
    }

    public bool GetCanSprint() { return canSprint; }

    public void SetCanSprint(bool canSprint) { this.canSprint = canSprint; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Player doesnt have rigidbody");
            enabled = false;
        }

        TryGetComponent(out Health health);

        if (health)
        {
            health.onDead += OnDied;
        }

        stamina = maxStamina;
    }

    void OnEnable()
    {
        // Create input class
        input = new GameActions();

        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCanceled;

        input.Enable();
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
        staminaUpdated?.Invoke(stamina);
    }

    void Update()
    {
        Sprint(KeyCode.LeftShift, KeyCode.W);
    }

    void FixedUpdate() 
    {
        Move(moveVector.y, moveVector.x);

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

    public void Move(float moveV, float moveH)
    {
        Vector3 moveDirection;

        moveDirection = transform.forward * moveV + transform.right * moveH;
        rb.AddForce(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Sprint(KeyCode RunInput, KeyCode WalkInput)
    {
        if (!canSprint) return;

        if (stamina > 0 && Input.GetKey(RunInput) && Input.GetKey(WalkInput))
        {
            SetIsSprinting(true);
            SetStamina(stamina - (staminaCost * Time.deltaTime));
        }
        else
        {
            SetIsSprinting(false);
            SetStamina(stamina + (staminaRecover * Time.deltaTime));
        }
    }

    void OnDied()
    {
        rb.freezeRotation = false;
        input.Disable();
    }
}