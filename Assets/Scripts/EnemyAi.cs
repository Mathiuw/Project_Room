using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyAi : MonoBehaviour
{
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

    bool isPatrolling = false;
    bool isAttacking = false;

    public bool canSeeTarget { get; private set; }
    bool canAttackTarget = false;

    public event Action onPatrol;
    public event Action onChase;
    public event Action onAttack;

    NavMeshAgent navMeshAgent;
    Health health;
    ShootGun shootGun;
    Collider[] rangeCheck;

    void OnDisable() => StopAllCoroutines();

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        shootGun = GetComponentInChildren<ShootGun>();

        waypoints = new Vector3[path.childCount];

        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
    }

    void Update() 
    {   
        FieldOfViewCheck();
        if (health.HealthAmount < health.MaxHealthAmount) canSeeTarget= true;
        Behavior();
    }

    void FieldOfViewCheck()
    {
        rangeCheck = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeCheck.Length != 0) 
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)) return;
            if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) return;

            canSeeTarget = true;
            if (distanceToTarget < radius / 1.1f) canAttackTarget = true;
        }
        else canAttackTarget = false;
    }

    void Behavior()
    {
        if (!canSeeTarget && !canAttackTarget) Patrol();
        if (canSeeTarget && !canAttackTarget) Chase();
        if (canSeeTarget && canAttackTarget) Attack();
    }

    void Patrol() 
    {      
        if (!isPatrolling) StartCoroutine(PatrolRoute());
    } 

    IEnumerator PatrolRoute()
    {
        isPatrolling= true;
        Debug.Log("Started Patrolling");
        onPatrol?.Invoke();

        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        Vector2 desiredPosition = new Vector2(waypoints[waypointIndex].x, waypoints[waypointIndex].z);
      
        while (true)
        {
            navMeshAgent.SetDestination(waypoints[waypointIndex]);

            while (position != desiredPosition)
            {              
                position = new Vector2(transform.position.x, transform.position.z);

                if (canSeeTarget)
                {
                    isPatrolling = false;
                    Debug.Log("Stopped Patrolling");
                    yield break;
                }
                yield return null;
            }

            waypointIndex++;
            if (waypointIndex == waypoints.Length) waypointIndex = 0;
            desiredPosition = new Vector2(waypoints[waypointIndex].x, waypoints[waypointIndex].z);

            yield return null;
        }
    }

    void Chase() 
    {
        navMeshAgent.SetDestination(Player.Instance.PlayerTransform().position);
        Debug.Log("Chasing");
        onChase?.Invoke();
    } 

    void Attack()
    {
        navMeshAgent.SetDestination(transform.position);
        transform.LookAt(Player.Instance.PlayerTransform().position);

        if (isAttacking) return;

        onAttack?.Invoke();
        StartCoroutine(ShootWeapon());
    }

    IEnumerator ShootWeapon()
    {
        isAttacking = true;
        Debug.Log("Started Attacking");

        yield return new WaitForSeconds(nextBurst);

        if (shootGun.ammo == 0) shootGun.ammo = shootGun.maximumAmmo;

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