using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    void Awake()
    {
        moveScript = gameObject.GetComponent<Movement>();
        jumpScript = gameObject.GetComponent<Jump>();
    }

    void Start()
    {
        playerScale = transform.localScale;
        Debug.Log(crouchOffset);
    }
    void Update()
    {
        Crouching();
    }

    [Header("Crouching")]
    [SerializeField]
    public Vector3 crouchScale = new Vector3(1,0.5f,1);
    private Vector3 playerScale;
    private Movement moveScript;
    private Jump jumpScript;
    public float crouchOffset;

    void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && jumpScript.isGrounded)
        {
            StartCrouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && jumpScript.isGrounded)
        {
            StopCrouch();
        }
    }

    void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - crouchOffset, transform.position.z);
    }

    void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + crouchOffset, transform.position.z);
    }
}
