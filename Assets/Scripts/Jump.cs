using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, ICanDo
{
    [Header("Can Do?")]
    [SerializeField] private bool canDo = true;

    [Header("Jump cost")]
    private Rigidbody rb;
    [SerializeField] private float jumpCost = 10;

    [Header("Ground Check")]
    public Transform checkSphereLocation;
    public LayerMask groundMask;
    public float sphereRadius;
    public bool isGrounded;

    void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    void Update()
    {
        groundCheck();
        Jumping();
    }

    //Jump
    [Header("Jump")]
    public float jumpForce;
    public float velDiv;

    void Jumping()
    {
        if (canDo)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded == true && Sprint.playerStamina - jumpCost >= 0)
                {
                    Debug.Log("Pulo");
                    rb.velocity /= velDiv;
                    rb.AddForce(transform.up * jumpForce);
                    Sprint.playerStamina -= jumpCost;
                    isGrounded = false;
                }
            }
        }
    }

    void groundCheck()
    {
        isGrounded = Physics.CheckSphere(checkSphereLocation.position, sphereRadius, groundMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(checkSphereLocation.position, sphereRadius);
    }

    public void CheckIfCanDo(bool check)
    {
        if (check)
        {
            canDo = false;
        }
        else canDo = true;
    }
}
