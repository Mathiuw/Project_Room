using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //Input class
    GameActions input;

    [SerializeField] float moveSpeed = 7.5f;

    public float sprintMultiplier { get; set; } = 1;
    Transform cameraTransform;
    Vector2 moveVector;
    Rigidbody rb;

    void OnEnable()
    {
        //Enable input
        input.Enable();

        //Add events
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
    }

    void OnDisable()
    {
        //Disable input
        input.Disable();
        //Remove events
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
    }

    void Awake() 
    {
        //Create input class
        input = new GameActions();

        //Get camera
        cameraTransform = Camera.main.transform;

        //Get rigidbody
        rb= GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        //Move player
        Move(moveVector.y, moveVector.x);

        //Rotate player body
        transform.localRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }

    public void Move(float moveV, float moveH)
    {
        Vector3 moveDirection;

        moveDirection = transform.forward * moveV + transform.right * moveH;

        rb.velocity = moveDirection.normalized * moveSpeed * sprintMultiplier; //* Time.deltaTime;
    }

    //Movement event functions
    void OnMovementPerformed(InputAction.CallbackContext value) 
    {
        moveVector = value.ReadValue<Vector2>();
    }

    void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }
}