using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Sprint : MonoBehaviour, ICanDo
{
    private bool canDo = true;

    public bool isInfinite = false;

    [Header("Sprinting")]
    [SerializeField] private int staminaLoss = 10;
    [SerializeField] private int staminaRecover = 8;
    [SerializeField] private float sprintMultiplier = 1.5f;

    public float GetSprintMultiplier { get => sprintMultiplier; private set => sprintMultiplier = value; }

    public float stamina { get; private set; } = 30;
    public float maximumStamina { get; private set; } = 30;

    public delegate IEnumerator OnInfiniteSprint(float time);
    public event OnInfiniteSprint infiniteSprint;

    void Update()
    {
        if (!canDo) return;
        Sprinting();
    }

    public void InfiniteSprintEvent(float time) => infiniteSprint?.Invoke(time);

    void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (stamina > 0 && Player.Instance.Jump.IsGrounded())
            {
                Player.Instance.Movement.sprintMultiplier = sprintMultiplier;

                if (isInfinite)
                {
                    stamina = maximumStamina;
                    Debug.Log("Infinite Sprinting");
                }
                else stamina -= staminaLoss * Time.deltaTime;
                Debug.Log("Sprinting");
            }
            else Player.Instance.Movement.sprintMultiplier = 1;
        }
        else
        {
            Player.Instance.Movement.sprintMultiplier = 1;
            if (stamina <= maximumStamina) stamina += staminaRecover * Time.deltaTime;
        }
        StaminaConstraintCheck();
    }

    public void RemoveStamina(float amomunt) { stamina -= amomunt; }

    void StaminaConstraintCheck() { if (stamina > maximumStamina) stamina = maximumStamina; }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}