using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health), typeof(Die))]
[RequireComponent(typeof(EnemyAi))]
[RequireComponent(typeof(EnemyAnimationManager))]
[RequireComponent(typeof(Ragdoll))]
public class Enemy : MonoBehaviour
{
    void Awake() => GetComponentInChildren<Die>().Died += OnDead;

    void OnDead() 
    {
        GetComponent<EnemyAi>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponentInChildren<Animator>().enabled = false;
        GetComponentInChildren<Ragdoll>().RagdollActive(true);
    }
}
