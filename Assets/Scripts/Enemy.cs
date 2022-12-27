using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Die))]
[RequireComponent(typeof(EnemyAi))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Ragdoll))]

public class Enemy : MonoBehaviour
{
    public Health health { get; private set; }
    public Die die { get; private set; }
    public EnemyAi enemyAi { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }
    public Ragdoll ragdoll { get; private set; }

    void Awake() 
    {
        health= GetComponentInChildren<Health>();
        die= GetComponentInChildren<Die>();
        enemyAi= GetComponentInChildren<EnemyAi>();
        navMeshAgent= GetComponentInChildren<NavMeshAgent>();
        ragdoll = GetComponentInChildren<Ragdoll>();

        die.OnDieBoolTrue += ragdoll.RagdollActive;
    }
}
