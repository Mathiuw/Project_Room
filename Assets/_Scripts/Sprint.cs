using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    [Header("Sprinting")]
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float multiplierWhileRunning = 1.5f;
    PlayerMovement playerMovement;

    public float stamina { get; private set; } = 30;
    public float maxStamina { get; private set; } = 30;
    public bool isInfinite = false;

    public event Action<float> staminaUpdated;

    void Start() 
    {
        playerMovement= GetComponent<PlayerMovement>();

        staminaUpdated?.Invoke(stamina);
    } 

    void Update() => Sprinting(KeyCode.LeftShift, KeyCode.W);

    public void Sprinting(KeyCode RunInput, KeyCode WalkInput)
    {
        if (Input.GetKey(RunInput) && Input.GetKey(WalkInput) && stamina > 0f)
        {
            playerMovement.sprintMultiplier = multiplierWhileRunning;

            if (isInfinite)
            {
                AddStamina(maxStamina * Time.deltaTime);
                Debug.Log("Infinite Sprinting");
                return;
            }

            else RemoveStamina(staminaCost * Time.deltaTime) ;
            Debug.Log("Sprinting");
            return;
        }
        else
        {
            playerMovement.sprintMultiplier = 1;

            if (stamina <= maxStamina) AddStamina(staminaRecover * Time.deltaTime);
        }
    }

    public void AddStamina(float amount)
    {
        stamina += amount;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        staminaUpdated?.Invoke(stamina);
    }

    public void RemoveStamina(float amomunt) 
    {
        stamina -= amomunt;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        staminaUpdated?.Invoke(stamina);
    }
}