using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    Animator animator;
    EnemyAi enemyAi;

    void Awake() 
    {
        animator = GetComponentInChildren<Animator>();
        enemyAi = GetComponent<EnemyAi>();
    }

    void Start() 
    {
        //Patrol animations
        enemyAi.onPatrol += StartWalk;
        //Chase animations
        enemyAi.onChase += StartWalk;
        enemyAi.onChase += StartAim;
        enemyAi.onChase += SpeedMultiplier;
        enemyAi.onStopChase += StopWalk;
        //Attack animations
        enemyAi.onAttack += StopWalk;
        enemyAi.onAttack += StartAim;
        enemyAi.onStopAttack += StartWalk;
    }

    void StartWalk() => animator.SetBool("walk", true);

    void StopWalk() => animator.SetBool("walk", false);

    void StartAim() => animator.SetBool("aim", true);

    void StopAim() => animator.SetBool("aim", false);

    void SpeedMultiplier() => animator.SetFloat("speed multiplier", enemyAi.GetSpeedMultiplier());
}
