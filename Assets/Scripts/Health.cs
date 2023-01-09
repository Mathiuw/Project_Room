using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die))]
public class Health : MonoBehaviour
{
    [SerializeField] int healthAmount = 100;
    [SerializeField] int maxHealthAmount = 100;

    public int HealthAmount { get => healthAmount; private set => healthAmount = value; }
    public int MaxHealthAmount { get => maxHealthAmount; private set => maxHealthAmount = value; }

    public event Action<int> healthUpdated;

    void Start() => healthUpdated?.Invoke(healthAmount);

    //Adiciona Vida
    public void AddHealth(int amount)
    {
        HealthAmount += amount;
        Mathf.Clamp(healthAmount, 0, maxHealthAmount);
        healthUpdated?.Invoke(healthAmount);
    }

    //Remove Vida e Checa Morte
    public void RemoveHealth(int amount)
    {
        HealthAmount -= amount;
        Mathf.Clamp(healthAmount, 0, maxHealthAmount);
        healthUpdated?.Invoke(healthAmount);
        Isdead();
    }

    //Checa Morte
    public bool Isdead()
    {
        if (HealthAmount <= 0)
        {
            GetComponent<Die>().Dead();
            return true;
        }
        else return false;
    }
}
