using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    void Awake() => GetComponentInChildren<Die>().onDead += OnDead;

    void OnDead() 
    {
        GetComponentInChildren<WeaponInteraction>().DropGun();
        GetComponentInChildren<EnemyAi>().enabled = false;
        GetComponentInChildren<NavMeshAgent>().enabled = false;
        GetComponentInChildren<Animator>().enabled = false;
        GetComponentInChildren<Ragdoll>().RagdollActive(true);
        GetComponentInChildren<Ragdoll>().transform.SetParent(transform);
        for (int i = 0; i < transform.childCount - 1; i++) Destroy(transform.GetChild(i).gameObject);
    }
}
