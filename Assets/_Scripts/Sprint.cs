using System;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    [Header("Sprinting")]
    [SerializeField] int staminaCost = 10;
    [SerializeField] int staminaRecover = 8;
    [SerializeField] float multiplier = 1.5f;
    PlayerMovement playerMovement;
    PlayerWeaponInteraction playerWeaponInteraction;

    public float stamina { get; private set; } = 30;
    public float maxStamina { get; private set; } = 30;
    public bool isRunning { get; private set; } = false;

    public event Action<float> staminaUpdated;

    void Awake() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponInteraction = GetComponent<PlayerWeaponInteraction>();
    } 

    void Start() => staminaUpdated?.Invoke(stamina);

    void Update() 
    {
        if (playerWeaponInteraction.isHoldingWeapon)
        {
            if (!CanRun(playerWeaponInteraction.isAiming, playerWeaponInteraction.currentWeapon.reloadGun.isReloading)) return;
        }

        if (!CanRun(playerWeaponInteraction.isAiming)) return;

        Sprinting(KeyCode.LeftShift, KeyCode.W);
    }

    public void AddStamina(float amount)
    {
        stamina += amount;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        staminaUpdated?.Invoke(stamina);
    }

    public void RemoveStamina(float amomunt)
    {
        stamina -= amomunt;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        staminaUpdated?.Invoke(stamina);
    }

    void SetRunState(bool b) 
    {
        isRunning = b;

        if(b) playerMovement.sprintMultiplier = multiplier;
        else playerMovement.sprintMultiplier = 1;
    }

    bool CanRun(bool isAiming, bool isReloading = false) 
    {
        if (isAiming || isReloading) 
        {
            SetRunState(false);
            RecoverStamina();
            return false;
        } 
        else return true;
    }

    void RecoverStamina() 
    {
        if (stamina <= maxStamina) AddStamina(staminaRecover * Time.deltaTime);
    }

    public void Sprinting(KeyCode RunInput, KeyCode WalkInput)
    {
        if (Input.GetKey(RunInput) && Input.GetKey(WalkInput))
        {
            if (stamina == 0) 
            {
                SetRunState(false);        
                return;
            } 

            SetRunState(true);
            RemoveStamina(staminaCost * Time.deltaTime);

            Debug.Log("Sprinting");
            return;
        }
        else 
        {
            SetRunState(false);
            RecoverStamina();
        }
    }
}