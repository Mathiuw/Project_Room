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

        enemyAi.onPatrol += ActivateWalk;
        enemyAi.onAttack += DisableWalk;
    }

    void ActivateWalk() => animator.SetFloat("walk", 1f);

    void DisableWalk() => animator.SetFloat("walk", 0f);

}
