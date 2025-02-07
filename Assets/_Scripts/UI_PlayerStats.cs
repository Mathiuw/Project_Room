using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    void Start() 
    {
        PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement)
        {
            SetStaminaUI(playerMovement.GetStamina());
            staminaBar.maxValue = playerMovement.GetMaxStamina();
            playerMovement.staminaUpdated += SetStaminaUI;

            Health health = playerMovement.GetComponent<Health>();

            if (health)
            {
                SetHealthUI(health.GetHealth());
                healthBar.maxValue = health.GetMaxHealth();

                health.healthUpdated += SetHealthUI;
            }
        }
        else Debug.LogError("Cant find Player");
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
