using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    private Slider healthBarSlider;

    [SerializeField] private GameObject staminaBar;
    [HideInInspector]public Slider staminaBarSlider;

    private void Awake()
    {
        staminaBarSlider = staminaBar.GetComponent<Slider>();
        staminaBarSlider.maxValue = Sprint.maximumStamina;

        healthBarSlider = healthBar.GetComponent<Slider>();
        healthBarSlider.maxValue = Health.maxHealth;
    }

    private void Update()
    {
        StaminaUI();
        HealthUI();
    }

    private void StaminaUI()
    {
        staminaBarSlider.value = Sprint.playerStamina;

        if (Sprint.playerStamina == Sprint.maximumStamina) staminaBar.SetActive(false);
        else staminaBar.SetActive(true);
    }

    private void HealthUI()
    {
        healthBarSlider.value = Health.playerHealth;

        if (healthBarSlider.value == Health.maxHealth) healthBar.SetActive(false);
        else healthBar.SetActive(true);
    }
}
