using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2000f;
    [SerializeField] float maxWalkingSpeed = 6f;
    [SerializeField] float maxRunningSpeed = 10f;
    public float sprintMultiplier { get; set; } = 1;
    Vector3 moveDirection;
    Rigidbody rb;

    void Awake() 
    {
        rb= GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
    }

    public void Move(float moveV, float moveH)
    {
        moveDirection = transform.forward * moveV + transform.right * moveH;

        rb.AddForce(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime, ForceMode.VelocityChange);

        MaxSpeedCheck();
    }

    void MaxSpeedCheck()
    {
        if (rb.velocity.magnitude > WhatMaxSpeed()) rb.velocity = Vector3.ClampMagnitude(rb.velocity, WhatMaxSpeed());
    }

    float WhatMaxSpeed()
    {
        if (sprintMultiplier > 1) return maxRunningSpeed;
        return maxWalkingSpeed;       
    }
}