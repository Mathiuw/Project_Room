﻿using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int health;
    bool isDead = false;

    public event Action<int> healthUpdated;
    public event Action onDead;

    public int GetHealth() { return health; }
    public int GetMaxHealth() { return maxHealth; }
    public bool GetIsDead() { return isDead; }

    void Awake() => maxHealth = health;

    void Start() 
    {
        healthUpdated?.Invoke(health);
    } 

    // Adiciona Vida
    public void AddHealth(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthUpdated?.Invoke(health);
    }

    // Remove Vida e Checa Morte
    public void RemoveHealth(int amount)
    {
        if (isDead) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthUpdated?.Invoke(health);

        if (health <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath() 
    {
        isDead = true;

        foreach (IDead deadInterface in GetComponents<IDead>())
        {
            deadInterface.Dead();
        }

        onDead?.Invoke();
    }
}