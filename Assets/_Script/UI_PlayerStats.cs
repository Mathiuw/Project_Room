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
            staminaBar.maxValue = playerMovement.MaxStamina;
            SetStaminaUI(playerMovement.Stamina);

            playerMovement.staminaUpdated += SetStaminaUI;


            Health health = playerMovement.GetComponent<Health>();

            if (health)
            {

                healthBar.maxValue = health.GetMaxHealth();
                SetHealthUI(health.GetHealth());

                health.healthUpdated += SetHealthUI;
            }
            else
            {
                Debug.LogError("Cant find player health");
            }
        }
        else Debug.LogError("Cant find PlayerMovement class");
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
    }
}
