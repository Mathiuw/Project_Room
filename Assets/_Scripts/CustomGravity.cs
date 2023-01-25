using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [SerializeField] float force;
    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate() 
    {
        SetGravity(); 
    }

    void SetGravity()          
    {
        rb.AddForce(Vector3.down * force * Time.deltaTime, ForceMode.VelocityChange);
    }
}
