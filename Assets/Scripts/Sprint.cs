using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Sprint : MonoBehaviour,ICanDo
{
    private bool canDo = true;

    public bool isInfinite = false;

    [Header("Sprinting")]
    private Movement moveScript;
    private Jump jumpScript;
    public float SprintMultiplier;
    [SerializeField] private int staminaLoss = 6;
    [SerializeField] private int staminaRecover = 8;

    public static float playerStamina = 30;
    public static float maximumStamina = 30;

    public delegate IEnumerator InfiniteSprintDelegate(float time);
    public event InfiniteSprintDelegate infiniteSprintEvent;

    public void Awake()
    {
        moveScript = GetComponent<Movement>();
        jumpScript = GetComponent<Jump>();
    }

    void Update()
    {
        if (!canDo) return;

        Sprinting();
        if (playerStamina > maximumStamina) playerStamina = maximumStamina;
    }

    void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (playerStamina > 0 && jumpScript.isGrounded)
            {
                moveScript.sprintMultiplier = SprintMultiplier;

                if (isInfinite)
                {
                    playerStamina = maximumStamina;
                    Debug.Log("Infinite Sprinting");
                } 
                else playerStamina -= staminaLoss * Time.deltaTime;
                Debug.Log("Sprinting");
            }
            else moveScript.sprintMultiplier = 1;
        }
        else
        {
            moveScript.sprintMultiplier = 1;
            if (playerStamina <= maximumStamina) playerStamina += staminaRecover * Time.deltaTime;
        }        
    }

    public void InfiniteSprint(float time)
    {
        if (infiniteSprintEvent != null)
        {
            StartCoroutine(infiniteSprintEvent(time));
        }       
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}