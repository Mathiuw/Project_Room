using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyAi))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public Health health { get; private set; }
    public EnemyAi enemyAi { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }

    void Awake() 
    {
        health= GetComponentInChildren<Health>();
        enemyAi= GetComponentInChildren<EnemyAi>();
        navMeshAgent= GetComponentInChildren<NavMeshAgent>();  
    }

}
