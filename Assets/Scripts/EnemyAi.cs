using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyAi : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] EnemyType enemyType;
    public EnemyState enemyState { get; private set; }

    [Header("Attack")]
    [SerializeField] Transform raycastPos;
    [SerializeField] int burst = 3;
    [SerializeField] float nextBurst = 0.5f;
    [SerializeField] float timeBetweenBullets = 0.1f;

    [Header("Patroling")]
    [SerializeField] Transform path;
    Vector3[] waypoints;
    int waypointIndex = 0;

    [Header("Field of view")]
    public float radius;
    [Range(0, 360)] public float angle;
    [SerializeField] LayerMask targetMask, obstructionMask;

    public event Action patrolled;
    public event Action chased;
    public event Action attacked;

    bool isAttacking = false;
    bool isPatrolling = false;

    NavMeshAgent navMeshAgent;
    Health health;
    ShootGun shootGun;
    Collider[] rangeCheck;

    enum EnemyType
    {
        Patrol,
        Stand, 
    }

    public enum EnemyState 
    {
        Patrolling,
        Chasing,
        Attacking, 
    }

    void OnDisable() => StopAllCoroutines();

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        shootGun = GetComponentInChildren<ShootGun>();

        if (enemyType == EnemyType.Stand) 
        {
            Destroy(path.gameObject);
            return;
        }

        waypoints = new Vector3[path.childCount];

        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
    }

    void Update() 
    {
        HealthCheck();
        FieldOfViewCheck();
        Behavior();
    }

    void HealthCheck() 
    {
        if (health.HealthAmount < health.MaxHealthAmount)
        {
            enemyState = EnemyState.Chasing;
            return;
        }
    }

    void FieldOfViewCheck()
    {
        rangeCheck = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeCheck.Length == 0) return;

        Transform target = rangeCheck[0].transform;
        Vector3 directtionToTarget = (target.position - transform.position).normalized;

        if (!(Vector3.Angle(transform.forward, directtionToTarget) < angle / 2)) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (Physics.Raycast(transform.position, directtionToTarget, distanceToTarget, obstructionMask)) return;

        enemyState = EnemyState.Chasing;

        if (distanceToTarget < radius / 1.1f) enemyState = EnemyState.Attacking;
        else enemyState = EnemyState.Chasing;
    }

    void Behavior()
    {
        if (enemyType != EnemyType.Stand && enemyState == EnemyState.Patrolling) Patrol();
        if (enemyState == EnemyState.Chasing) Chase();
        if (enemyState == EnemyState.Attacking) Attack();
    }

    void Patrol() 
    {      
        if (!isPatrolling) StartCoroutine(PatrolRoute());
    } 

    IEnumerator PatrolRoute()
    {
        isPatrolling= true;
        Debug.Log("Started Patrolling");
        patrolled?.Invoke();

        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        Vector2 desiredPosition = new Vector2(waypoints[waypointIndex].x, waypoints[waypointIndex].z);

        navMeshAgent.SetDestination(waypoints[waypointIndex]);

        while (position != desiredPosition)
        {
            position = new Vector2(transform.position.x, transform.position.z);

            if (enemyState != EnemyState.Patrolling) 
            {
                isPatrolling = false;
                Debug.Log("Stopped Patrolling");
                yield break;
            } 
            yield return null;
        }

        waypointIndex++;
        if (waypointIndex == waypoints.Length) waypointIndex = 0;

        isPatrolling = false;
        Debug.Log("Stopped Patrolling");
        yield break;
    }

    void Chase() 
    {
        navMeshAgent.SetDestination(Player.Instance.PlayerTransform().position);
        Debug.Log("Chasing");
        chased?.Invoke();
    } 

    void Attack()
    {
        navMeshAgent.SetDestination(transform.position);
        transform.LookAt(Player.Instance.PlayerTransform().position);

        if (isAttacking) return;

        attacked?.Invoke();
        StartCoroutine(ShootWeapon(isAttacking));
    }

    IEnumerator ShootWeapon(bool isStarted)
    {
        isAttacking = true;
        Debug.Log("Started Attacking");

        yield return new WaitForSeconds(nextBurst);

        for (int i = 0; i < burst; i++)
        {
            shootGun.Shooting(raycastPos);
            yield return new WaitForSeconds(timeBetweenBullets);
        }

        isAttacking = false;
        Debug.Log("Stopped Attacking");
        yield break;
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = path.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in path)
        {
            Gizmos.DrawSphere(waypoint.position, 0.5f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}