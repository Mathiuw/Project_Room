using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    Animator animator;
    EnemyAi enemyAi;

    void Awake() 
    {
        animator = GetComponentInChildren<Animator>();   
        enemyAi= GetComponent<EnemyAi>();

        enemyAi.onPatrol += StartWalk;
        enemyAi.onChase += StartWalk;
        enemyAi.onChase += StartAim;
        enemyAi.onAttack += StopWalk;
        enemyAi.onAttack += StartAim;
    }

    void StartWalk() => animator.SetFloat("walk", 1f);

    void StopWalk() => animator.SetFloat("walk", 0f);

    void StartAim() => animator.SetBool("Aim", true);

}
