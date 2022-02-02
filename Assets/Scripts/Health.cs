using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static int playerHealth = 100;
    public static int maxHealth = 100;

    public static bool playerDead = false;

    private Rigidbody rb;

    [Header("Components that will be disabled")]
    [SerializeField] private Component[] components;

    [Header("Hud Elements that will be disabled")]
    [SerializeField] private GameObject[] HUDElements;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (playerDead)
        {
            Die();          
        }
    }

    //Adiciona vida ao Player
    public static void AddHealth(int amount)
    {
        playerHealth += amount;
        MaxHealthCheck();
    }

    //Remove vida do Player e checa se ele esta morto 
    public static void RemoveHealth(int amount)
    {
        playerHealth -= amount;
        MaxHealthCheck();
        Isdead();
    }

    //Morre(Obvio)
    void Die()
    {
        rb.freezeRotation = false;

        CursorState.CursorUnlock();

        foreach (MonoBehaviour c in components)
        {
            c.enabled = false;
        }

        foreach (GameObject g in HUDElements)
        {
            g.SetActive(false);
        }
    }

    private static void MaxHealthCheck()
    {
        if (playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }

    public static void Isdead()
    {
        if (playerHealth <= 0)
        {
            playerDead = true;
        }           
        else
            return;
    }
}
