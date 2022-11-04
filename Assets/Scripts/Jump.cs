using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Jump : MonoBehaviour, ICanDo
{
    private bool canDo = true;

    [Header("Jump")]
    public float jumpForce;
    public float velDiv;

    [Header("Stamina Jump Cost")]
    [SerializeField] private float jumpCost = 10;

    [Header("Ground Check")]
    [SerializeField] Transform checkSphereLocation;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float sphereRadius;

    void Awake() => FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;

    void Update()
    {
        if (!canDo) return;
        if (Input.GetKeyDown(KeyCode.Space)) Jumping();
    }

    void Jumping()
    {
        if (IsGrounded() && Player.Instance.Sprint.stamina - jumpCost >= 0)
        {
            Debug.Log("Pulo");
            Player.Instance.RigidBody.velocity /= velDiv;
            Player.Instance.RigidBody.AddForce(transform.up * jumpForce);
            Player.Instance.Sprint.RemoveStamina(jumpCost);
        }
    }

    public bool IsGrounded() => Physics.CheckSphere(checkSphereLocation.position, sphereRadius, groundMask);

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
