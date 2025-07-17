using UnityEngine;

public class Attack : IState
{
    private readonly EnemyAi _enemyAi;

    public Attack(EnemyAi enemyAi) 
    {  
        _enemyAi = enemyAi; 
    }

    public void OnEnter()
    {
        _enemyAi.StartShooting();
    }

    public void OnExit()
    {
        _enemyAi.StopShooting();
    }

    public void Tick()
    {
        _enemyAi.LookToTarget();
    }
}
