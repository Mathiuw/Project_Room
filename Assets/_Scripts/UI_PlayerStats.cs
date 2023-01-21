using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;
    Health health;
    Sprint sprint;

    void Start() 
    {
        health = Player.Instance.GetComponentInChildren<Health>();
        sprint= Player.Instance.GetComponentInChildren<Sprint>();

        staminaBar.maxValue = sprint.maxStamina;
        healthBar.maxValue = health.MaxHealthAmount;

        health.healthUpdated += SetHealthUI;
        sprint.staminaUpdated += SetStaminaUI;
    }

    void SetStaminaUI(float stamina)
    {
        staminaBar.value = stamina;

        if (staminaBar.value == staminaBar.maxValue) staminaBar.gameObject.SetActive(false);
        else staminaBar.gameObject.SetActive(true);
    }

    void SetHealthUI(int healthAmount)
    {
        healthBar.value = healthAmount;

        if (healthBar.value == healthBar.maxValue) healthBar.gameObject.SetActive(false);
        else healthBar.gameObject.SetActive(true);
    }
}
