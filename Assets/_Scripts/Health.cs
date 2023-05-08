using System;
using UnityEngine;

[RequireComponent(typeof(Die))]
public class Health : MonoBehaviour
{
    [SerializeField] int healthAmount;
    int maxHealthAmount;

    public int HealthAmount { get => healthAmount; private set => healthAmount = value; }
    public int MaxHealthAmount { get => maxHealthAmount; private set => maxHealthAmount = value; }

    public event Action<int> healthUpdated;

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
        HealthAmount -= amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthAmount);
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
