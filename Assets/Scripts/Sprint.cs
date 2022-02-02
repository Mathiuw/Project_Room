using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    [Header("Sprinting")]
    private Movement moveScript;
    private Jump jumpScript;
    public float SprintMultiplier;
    [SerializeField] private int staminaLoss = 6;
    [SerializeField] private int staminaRecover = 8;

    public static float playerStamina = 30;
    public static float maximumStamina = 30;

    public void Awake()
    {
        moveScript = GetComponent<Movement>();
        jumpScript = GetComponent<Jump>();
    }

    void Update()
    {
        Sprinting();

        if (playerStamina > maximumStamina)
            playerStamina = maximumStamina;
    }

    void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (playerStamina > 0 && jumpScript.isGrounded)
            {
                moveScript.sprintMultiplier = SprintMultiplier;
                playerStamina -= staminaLoss * Time.deltaTime;
            }
            else
            {
                moveScript.sprintMultiplier = 1;
            }          
        }
        else
        {
            moveScript.sprintMultiplier = 1;

            if (playerStamina <= maximumStamina)            
                playerStamina += staminaRecover * Time.deltaTime;
        }        
    }
}