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
        healthBarSlider = healthBar.GetComponent<Slider>();
    }

    void Start() 
    {
        staminaBarSlider.maxValue = Player.Instance.Sprint.maximumStamina;
        healthBarSlider.maxValue = Player.Instance.Health.MaxHealthAmount;
    }

    void Update()
    {
        SetStaminaUI();
        SetHealthUI();
    }

    void SetStaminaUI()
    {
        staminaBarSlider.value = Player.Instance.Sprint.stamina;

        if (Player.Instance.Sprint.stamina == Player.Instance.Sprint.maximumStamina) staminaBar.SetActive(false);
        else staminaBar.SetActive(true);
    }

    private void SetHealthUI()
    {
        healthBarSlider.value = Player.Instance.Health.HealthAmount;

        if (healthBarSlider.value == Player.Instance.Health.MaxHealthAmount) healthBar.SetActive(false);
        else healthBar.SetActive(true);
    }
}
