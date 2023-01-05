using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class Die : MonoBehaviour
{
    public event Action Died;

    [Header("Components that will be disabled")]
    [SerializeField] private Component[] components;

    //Morre(Obvio)
    public void Dead() 
    {
        Died?.Invoke();
        DisableComponents(); 
    }

    //Desativa os Componentes do Objeto
    private void DisableComponents()
    { 
        foreach (MonoBehaviour c in components) c.enabled = false;     
    }
}
