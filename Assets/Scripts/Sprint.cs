using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    //Player Sprint
    [Header("Sprinting")]
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float Multiplier = 1.5f;
    public bool isInfinite = false;     

    //Player Stamina
    public float stamina { get; private set; } = 30;
    public float maximumStamina { get; private set; } = 30;

    //Infinite Stamina Event
    public event Func<float, IEnumerator> onInfiniteSprint;

    public void InfiniteSprintEvent(float time) => onInfiniteSprint?.Invoke(time);

    public void Sprinting(KeyCode RunInput, KeyCode WalkInput)
    {
        Player player = Player.Instance;

        if (Input.GetKey(RunInput) && Input.GetKey(WalkInput))
        {
            if (stamina <= 0) return;

            player.Movement.sprintMultiplier = Multiplier;

            if (isInfinite)
            {
                stamina = maximumStamina;

                Debug.Log("Infinite Sprinting");
                return;
            }

            else stamina -= staminaCost * Time.deltaTime;

            Debug.Log("Sprinting");
            return;
        }

        player.Movement.sprintMultiplier = 1;
        if (stamina <= maximumStamina) AddStamina(staminaRecover * Time.deltaTime);
        StaminaConstraintCheck();
    }

    public void RemoveStamina(float amomunt) => stamina -= amomunt;

    public void AddStamina(float amount) => stamina += amount;

    void StaminaConstraintCheck() { if (stamina > maximumStamina) stamina = maximumStamina; }
}