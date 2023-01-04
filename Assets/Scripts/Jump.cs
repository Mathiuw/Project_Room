using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump")]
    public float jumpForce = 20;

    [Header("Stamina Jump Cost")]
    [SerializeField] private float jumpCost = 10;

    public void Jumping()
    {
        if (Player.Instance.IsGrounded() && Player.Instance.Sprint.stamina - jumpCost >= 0)
        {
            Player.Instance.RigidBody.AddForce(transform.up * jumpForce , ForceMode.VelocityChange);
            Player.Instance.Sprint.RemoveStamina(jumpCost);
            Debug.Log("Pulou");
        }
    }
}
