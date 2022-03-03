using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour,ICanDo
{
    [Header("Can Move?")]
    [SerializeField] private bool canDo = true;

    [Header("Movimento")]
    private Rigidbody rb;
    public float moveSpeed = 2000f;
    public float maxWalkingSpeed = 6f;
    public float maxRunningSpeed = 10f;
    public float extraGrav = 20f;
    [HideInInspector] public float sprintMultiplier;
    [HideInInspector] public float airMultiplier;
    private float moveV, moveH;
    private Vector3 moveDirection;

    [Header("Drag control")]
    public float rbDrag;
    public float inAir;
    Jump jumpScript;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpScript = GetComponent<Jump>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    void Update()
    {
        DragControl();
    }

    void FixedUpdate()
    {
        if (canDo)
        {
            Move();
            MaxSpeedCheck();
        }
    }

    void Move()
    {
        rb.AddForce(Vector3.down * extraGrav * Time.deltaTime);

        moveV = Input.GetAxisRaw("Vertical");
        moveH = Input.GetAxisRaw("Horizontal");

        moveDirection = transform.forward * moveV + transform.right * moveH;

        rb.AddForce(moveDirection.normalized * moveSpeed * sprintMultiplier * airMultiplier * Time.deltaTime);
    }

    float MaxSpeed()
    {
        if (sprintMultiplier > 1)
        {
            return maxRunningSpeed;
        }
        else
        {
            return maxWalkingSpeed;
        }       
    }

    void MaxSpeedCheck()
    {
        if (rb.velocity.magnitude > MaxSpeed())
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed());
        }
    }

    void DragControl()
    {
        if (jumpScript.isGrounded)
        {
            rb.drag = rbDrag;
            airMultiplier = 1;
            Debug.Log("Grounded");
        }
        else
        {
            rb.drag = 0;
            airMultiplier = inAir;
            Debug.Log("!Grounded");
        }
    }

    public void CheckIfCanDo(bool check)
    {
        if (check)
        {
            rb.velocity = Vector3.zero;
            canDo = false;
        }
        else canDo = true;
    }
}