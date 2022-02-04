using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private Rigidbody rb;

    [Header("Patroling")]
    [SerializeField] private Transform pathHolder;
    [SerializeField] private Vector3[] waypoints;
    [SerializeField] private float waitTime;
    private bool startedPatroling = false; 

    [Header("Field of view")]
    public float radius;
    [Range(0,360)]
    public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public bool canAttackPlayer;


    [Header("Health")]    
    public int health = 100;

    [Header("Attack")]
    [SerializeField] private int burst = 5, nextBurst = 2;
    [SerializeField] private float timeBetweenBullets = 0.1f;
    private bool startedAttacking = false;
    private ShootGun gunScript;

    private void Awake()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.isKinematic = true;

        waypoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < pathHolder.childCount; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y ,waypoints[i].z);
        }

        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        if (!IsDead())
        {
            if (!canSeePlayer && !canAttackPlayer) Patroling();
            if (canSeePlayer && !canAttackPlayer || health < 100 && !canAttackPlayer) ChasePlayer();
            if (canSeePlayer && canAttackPlayer) AttackPlayer();
        }
        else
        {
            StopAllCoroutines();
            agent.enabled = false;
            rb.isKinematic = false;
        }
    }

    private void Patroling()
    {
        if (!startedPatroling) StartCoroutine(PatrolRoute());
    }

    IEnumerator PatrolRoute()
    {
        startedPatroling = true;

        int waypointIndex = 0;

        agent.SetDestination(waypoints[waypointIndex]);

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
                agent.SetDestination(waypoints[waypointIndex]);
            }
            yield return null;
        }
    }

    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
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
                    canSeePlayer = true;

                    if (distanceToTarget < radius / 1.1f)
                        canAttackPlayer = true;
                    else
                        canAttackPlayer = false;
                }
                else
                    canSeePlayer = false;                            
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            canAttackPlayer = false;
        }
    }

    private void ChasePlayer()
    {
        ResetCoRoutines();
        agent.SetDestination(player.transform.position);
    }

    IEnumerator AttackRoutine()
    {
        startedAttacking = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < burst; i++)
        {
            gunScript.EnemyShoot(transform);
            yield return new WaitForSeconds(timeBetweenBullets);
        }
        yield return new WaitForSeconds(nextBurst);
        startedAttacking = false;
        yield return null;
    }

    private void AttackPlayer()
    {
        gunScript = GetComponentInChildren<ShootGun>();

        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        if (gunScript != null)
        {
            if (!startedAttacking) StartCoroutine(AttackRoutine());
        }
    }

    public bool IsDead()
    {
        if (health <= 0)
        {
            return true;
        }
        else return false;
    }

    private void ResetCoRoutines()
    {
        StopCoroutine(PatrolRoute());
        StopCoroutine(AttackRoutine());

        startedAttacking = false;
        startedPatroling = false;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;
    }

    private void OnDrawGizmos()
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