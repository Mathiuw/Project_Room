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
    public bool isInfinite { get; private set; } = false;

    bool isAiming = false;
    bool isReloading = false;

    public event Action<float> staminaUpdated;

    void Awake() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponInteraction = GetComponent<PlayerWeaponInteraction>();
    } 

    void Start() => staminaUpdated?.Invoke(stamina);

    void Update() 
    {
        isAiming = playerWeaponInteraction.isAiming;
        if (playerWeaponInteraction.isHoldingWeapon) isReloading = playerWeaponInteraction.currentWeapon.reloadGun.isReloading;

        Sprinting(KeyCode.LeftShift, KeyCode.W);
    } 

    void SetRun(bool b) 
    {
        if(isRunning != b)isRunning = b;

        if(b) playerMovement.sprintMultiplier = multiplier;
        else playerMovement.sprintMultiplier = 1;
    }

    bool CanRun() 
    {
        if (isAiming || isReloading) return false;
        else return true;
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

    public void Sprinting(KeyCode RunInput, KeyCode WalkInput)
    {
        SetRun(false);

        if (Input.GetKey(RunInput) && Input.GetKey(WalkInput) && CanRun())
        {
            if (stamina == 0) return;

            SetRun(true);

            if (isInfinite)
            {
                AddStamina(maxStamina * Time.deltaTime);
                Debug.Log("Infinite Sprinting");
                return;
            }
            else RemoveStamina(staminaCost * Time.deltaTime);

            Debug.Log("Sprinting");
            return;
        }
        else 
        {
            if (stamina <= maxStamina) AddStamina(staminaRecover * Time.deltaTime);
        }
    }
}