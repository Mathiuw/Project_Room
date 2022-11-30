using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die))]
public class Health : MonoBehaviour
{
    [SerializeField] private int healthAmount = 100;
    [SerializeField] private int maxHealthAmount = 100;

    public int HealthAmount { get => healthAmount; private set => healthAmount = value; }
    public int MaxHealthAmount { get => maxHealthAmount; private set => maxHealthAmount = value; }

    //Adiciona Vida
    public void AddHealth(int amount)
    {
        HealthAmount += amount;
        MaxConstraintCheck();
    }

    //Remove Vida e Checa Morte
    public void RemoveHealth(int amount)
    {
        HealthAmount -= amount;
        MaxConstraintCheck();
        Isdead();
    }

    //Checa Se a Vida Esta Fora Dos Parametros
    void MaxConstraintCheck()
    {
        if (HealthAmount > MaxHealthAmount) HealthAmount = MaxHealthAmount;
        else if (HealthAmount < 0) HealthAmount = 0;
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
