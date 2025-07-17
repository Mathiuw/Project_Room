using UnityEngine.AI;

public class Chase : IState
{
    private readonly EnemyAi _enemyAi;
    private readonly NavMeshAgent _navMeshAgent;

    public Chase(EnemyAi enemyAi, NavMeshAgent navMeshAgent) 
    {
        _enemyAi = enemyAi;
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_enemyAi.Target.position);
        _enemyAi.Run(true);
    }

    public void OnExit()
    {
        _enemyAi.Run(false);
        _navMeshAgent.enabled = false;
    }

    public void Tick()
    {
        
    }
}
