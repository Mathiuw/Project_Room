using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    void Awake() => GetComponentInChildren<Die>().Died += OnDead;

    void OnDead() 
    {
        GetComponentInChildren<WeaponInteraction>().DropGun();
        GetComponentInChildren<EnemyAi>().enabled = false;
        GetComponentInChildren<NavMeshAgent>().enabled = false;
        GetComponentInChildren<Animator>().enabled = false;
        GetComponentInChildren<Ragdoll>().RagdollActive(true);
        GetComponentInChildren<Ragdoll>().transform.SetParent(null);
        Destroy(gameObject);
    }
}
