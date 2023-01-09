using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] EnemyType enemyType;

    [Header("Attack")]
    [SerializeField] Transform raycastPos;
    [SerializeField] int burst = 5, nextBurst = 2;
    [SerializeField] float timeBetweenBullets = 0.1f;
    bool startedAttacking = false;
    NavMeshAgent navMeshAgent;
    Health health;
    ShootGun shootGun;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")]
    [SerializeField] Transform pathHolder;
    [SerializeField] Vector3[] waypoints;
    [SerializeField] float waitTime;
     bool startedPatroling = false; 

    [Header("Field of view")]
    public float radius;
    [Range(0,360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool SawPlayer;
    public bool canAttackPlayer;

    enum EnemyType
    {
        Patroling,
        Standing,
    }

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        if (enemyType != EnemyType.Standing)
        {
            waypoints = new Vector3[pathHolder.childCount];

            for (int i = 0; i < pathHolder.childCount; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
                waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
            }
        }
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {
        if (enemyType != EnemyType.Standing) if (!SawPlayer && !canAttackPlayer) Patroling();
        if ((SawPlayer && !canAttackPlayer) || health.HealthAmount < health.MaxHealthAmount ) ChasePlayer();
        if (SawPlayer && canAttackPlayer) AttackPlayer();
    }

    void OnDisable() 
    {
        StopAllCoroutines();    
    }

    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
            yield return null;
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directtionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directtionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directtionToTarget, distanceToTarget, obstructionMask))
                {
                    SawPlayer = true;

                    if (distanceToTarget < radius / 1.1f) canAttackPlayer = true;
                    else canAttackPlayer = false;
                }
                else canAttackPlayer = false;
            }
        }
        else if (SawPlayer) canAttackPlayer = false;       
    }

    void Patroling() { if (!startedPatroling) StartCoroutine(PatrolRoute()); }

    IEnumerator PatrolRoute()
    {
        startedPatroling = true;

        int waypointIndex = 0;

        navMeshAgent.SetDestination(waypoints[waypointIndex]);

        while (true)
        {
            if (transform.position.x == waypoints[waypointIndex].x && transform.position.z == waypoints[waypointIndex].z)
            {
                waypointIndex++;
                if (waypointIndex == waypoints.Length)
                {
                    waypointIndex = 0;
                }
                yield return new WaitForSeconds(waitTime);
                navMeshAgent.SetDestination(waypoints[waypointIndex]);
            }
            if (SawPlayer)
            {
                startedPatroling = false;
                yield break;
            }
            yield return null;
        }
    }

    void ChasePlayer() => navMeshAgent.SetDestination(Player.Instance.PlayerTransform().position);

    void AttackPlayer()
    {
        shootGun = GetComponentInChildren<ShootGun>();

        navMeshAgent.SetDestination(transform.position);
        transform.LookAt(Player.Instance.PlayerTransform().position);

        if (shootGun != null && !startedAttacking) StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        startedAttacking = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < burst; i++)
        {
            shootGun.Shooting(raycastPos);
            yield return new WaitForSeconds(timeBetweenBullets);
        }
        startedAttacking = false;

        if (!canAttackPlayer) yield break;
        yield return new WaitForSeconds(nextBurst);
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.5f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}