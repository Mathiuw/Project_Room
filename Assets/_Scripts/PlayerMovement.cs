using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 200.0f;
    float sprintMultiplier = 1;
    Transform playerCameraTransform;
    Player player;
    Vector2 moveVector;
    Rigidbody rb;

    [Header("Sprint")]
    [SerializeField] bool canSprint = true;
    [SerializeField] float maxStamina = 30;
    float stamina;
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float multiplier = 1.5f;
    bool isSprinting = false;

    // Stamina update event
    public event Action<float> staminaUpdated;

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
    public void SetIsSprinting(bool isSprinting)
    {
        this.isSprinting = isSprinting;

        if (isSprinting) SetSprintMultiplier(multiplier);
        else SetSprintMultiplier(1);
    }

    void OnEnable()
    {
        // Add events
        player.GetInput().Player.Movement.performed += OnMovementPerformed;
        player.GetInput().Player.Movement.canceled += OnMovementCanceled;
    }

    void OnDisable()
    {
        // Remove events
        player.GetInput().Player.Movement.performed -= OnMovementPerformed;
        player.GetInput().Player.Movement.canceled -= OnMovementCanceled;
    }

    void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerCameraTransform = player.GetPlayerCamera().transform;

        staminaUpdated?.Invoke(stamina);
    }

    void Update()
    {
        Sprint(KeyCode.LeftShift, KeyCode.W);
    }

    void FixedUpdate() 
    {
        // Move player
        Move(moveVector.y, moveVector.x);

        // Rotate player body
        transform.localRotation = Quaternion.Euler(0, playerCameraTransform.eulerAngles.y, 0);
    }

    public void Move(float moveV, float moveH)
    {
        Vector3 moveDirection;

        moveDirection = transform.forward * moveV + transform.right * moveH;
        rb.velocity = moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime;
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

    // Movement event functions
    void OnMovementPerformed(InputAction.CallbackContext value) 
    {
        moveVector = value.ReadValue<Vector2>();
    }

    void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

}