using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    Slider healthBarSlider;

    [SerializeField] GameObject staminaBar;
    [HideInInspector]public Slider staminaBarSlider;

    Player player;

    private void Awake()
    {
        staminaBarSlider = staminaBar.GetComponent<Slider>();
        healthBarSlider = healthBar.GetComponent<Slider>();
    }

    void Start() 
    {
        player = Player.Instance;

        staminaBarSlider.maxValue = player.Sprint.maximumStamina;
        healthBarSlider.maxValue = player.Health.MaxHealthAmount;
    }

    void Update()
    {
        SetStaminaUI();
        SetHealthUI();
    }

    void SetStaminaUI()
    {
        staminaBarSlider.value = player.Sprint.stamina;

        if (player.Sprint.stamina == player.Sprint.maximumStamina) staminaBar.SetActive(false);
        else staminaBar.SetActive(true);
    }

    private void SetHealthUI()
    {
        healthBarSlider.value = player.Health.HealthAmount;

        if (healthBarSlider.value == player.Health.MaxHealthAmount) healthBar.SetActive(false);
        else healthBar.SetActive(true);
    }
}
