using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    [Header("Sprinting")]
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float Multiplier = 1.5f;
    [HideInInspector] public bool isInfinite = false;     

    public float stamina { get; private set; } = 30;
    public float maxStamina { get; private set; } = 30;

    public event Action<float> staminaUpdated;

    void Start() => staminaUpdated?.Invoke(stamina);

    public void Sprinting(KeyCode RunInput, KeyCode WalkInput)
    {
        Player player = Player.Instance;

        if (Input.GetKey(RunInput) && Input.GetKey(WalkInput) && stamina > 0f)
        {
            player.Movement.sprintMultiplier = Multiplier;

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

        player.Movement.sprintMultiplier = 1;
        if (stamina <= maxStamina) AddStamina(staminaRecover * Time.deltaTime);
    }

    public void AddStamina(float amount)
    {
        stamina += amount;
        Mathf.Clamp(stamina, 0f, maxStamina);
        staminaUpdated?.Invoke(stamina);
    }

    public void RemoveStamina(float amomunt) 
    {
        stamina -= amomunt;
        Mathf.Clamp(stamina, 0f, maxStamina);
        staminaUpdated?.Invoke(stamina);
    }
}