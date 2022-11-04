using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Die))]
public class Health : MonoBehaviour
{
    [SerializeField] private int healthAmount = 100;
    [SerializeField] private int maxHealthAmount = 100;

    public int health { get => healthAmount; private set => healthAmount = value; }
    public int maxHealth { get => maxHealthAmount; private set => maxHealthAmount = value; }

    //Adiciona Vida
    public void AddHealth(int amount)
    {
        health += amount;
        MaxConstraintCheck();
    }

    //Remove Vida e Checa Morte
    public void RemoveHealth(int amount)
    {
        health -= amount;
        MaxConstraintCheck();
        Isdead();
    }

    //Checa Se a Vida Esta Fora Dos Parametros
    void MaxConstraintCheck()
    {
        if (health > maxHealth) health = maxHealth;
        else if (health < 0) health = 0;
    }

    //Checa Morte
    public bool Isdead()
    {
        if (health <= 0)
        {
            GetComponent<Die>().Dead();
            return true;
        }
        else return false;
    }
}
