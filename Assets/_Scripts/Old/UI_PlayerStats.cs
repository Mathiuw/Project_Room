using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    void Start() 
    {
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            Health health = player.GetComponent<Health>();
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

            SetHealthUI(health.GetHealth());
            SetStaminaUI(playerMovement.GetStamina());

            healthBar.maxValue = health.GetMaxHealth();
            staminaBar.maxValue = playerMovement.GetMaxStamina();

            health.healthUpdated += SetHealthUI;
            playerMovement.staminaUpdated += SetStaminaUI;
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
