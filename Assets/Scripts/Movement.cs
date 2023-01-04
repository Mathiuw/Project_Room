using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] float moveSpeed = 2000f;
    [SerializeField] float maxWalkingSpeed = 6f;
    [SerializeField] float maxRunningSpeed = 10f;
    public float sprintMultiplier { get; set; } = 1;
    Vector3 moveDirection;

    public void Move(float moveV, float moveH)
    {
        moveDirection = transform.forward * moveV + transform.right * moveH;

        Player.Instance.RigidBody.AddForce(moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime);

        MaxSpeedCheck();
    }

    void MaxSpeedCheck()
    {
        Rigidbody rb = Player.Instance.RigidBody;

        if (rb.velocity.magnitude > WhatMaxSpeed()) rb.velocity = Vector3.ClampMagnitude(rb.velocity, WhatMaxSpeed());
    }

    float WhatMaxSpeed()
    {
        if (sprintMultiplier > 1) return maxRunningSpeed;
        return maxWalkingSpeed;       
    }
}