using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int healthAmount;
    int maxHealthAmount;

    public int HealthAmount { get => healthAmount; private set => healthAmount = value; }
    public int MaxHealthAmount { get => maxHealthAmount; private set => maxHealthAmount = value; }
    public bool isDead { get; private set; }

    public event Action<int> healthUpdated;
    public event Action onDead;

    void Awake() => maxHealthAmount = healthAmount;

    void Start() 
    {
        healthUpdated?.Invoke(healthAmount);
    } 

    //Adiciona Vida
    public void AddHealth(int amount)
    {
        HealthAmount += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthAmount);
        healthUpdated?.Invoke(healthAmount);
    }

    //Remove Vida e Checa Morte
    public void RemoveHealth(int amount)
    {
        if (isDead) return;

        HealthAmount -= amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthAmount);
        healthUpdated?.Invoke(healthAmount);
        Isdead();
    }

    //Checa Morte
    bool Isdead()
    {
        if (HealthAmount <= 0)
        {
            isDead = true;
            onDead?.Invoke();
            return true;
        }
        else return false;
    }
}
