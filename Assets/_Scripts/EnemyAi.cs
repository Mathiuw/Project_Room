using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyAi : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] Transform raycastPos;
    [SerializeField] int burst = 3;
    [SerializeField] float nextBurst = 1f;
    [SerializeField] float timeBetweenBullets = 0.1f;

    [Header("Patroling")]
    [SerializeField] Transform path;
    Vector3[] waypoints;
    Vector3 target;
    int waypointIndex = 0;

    [Header("Field of view")]
    public float radius;
    [Range(0, 360)] public float angle;
    [SerializeField] LayerMask targetMask, obstructionMask;

    public bool isPatrolling { get; private set; }
    public bool isAttacking { get; private set; }
    public bool canSeeTarget { get; private set; }
    public bool canAttackTarget { get; private set; }

    public event Action onPatrol;
    public event Action onChase;
    public event Action onAttack;

    NavMeshAgent navMeshAgent;
    Health health;
    EnemyWeaponInteraction enemyWeaponInteraction;
    Collider[] rangeCheck;

    void OnDisable() => StopAllCoroutines();

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        enemyWeaponInteraction = GetComponent<EnemyWeaponInteraction>();
        
        waypoints = new Vector3[path.childCount]; 

        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
    }

    void Update() 
    {
        target = Player.instance.transform.position;

        FieldOfViewCheck();
        if (health.HealthAmount < health.MaxHealthAmount) canSeeTarget = true;
        Behavior();
    }

    void FieldOfViewCheck()
    {
        canAttackTarget = false;

        rangeCheck = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeCheck.Length == 0) return;

        Transform target = rangeCheck[0].transform;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (!(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)) return;
        if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) return;

        canSeeTarget = true;
        if (distanceToTarget < radius / 1.1f) canAttackTarget = true;
    }

    void Behavior()
    {
        if (!canSeeTarget && !canAttackTarget && !isPatrolling) StartCoroutine(PatrolRoute(navMeshAgent, waypoints));
        if (canSeeTarget && !canAttackTarget) Chase(navMeshAgent, target);
        if (canSeeTarget && canAttackTarget) 
        {
            transform.LookAt(target);
            if (!isAttacking) StartCoroutine(ShootWeapon(navMeshAgent, enemyWeaponInteraction));
        } 
    }

    IEnumerator PatrolRoute(NavMeshAgent navMeshAgent, Vector3[] waypoints)
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

    void Chase(NavMeshAgent navMeshAgent, Vector3 target) 
    {
        navMeshAgent.SetDestination(target);
        Debug.Log("Chasing");
        onChase?.Invoke();
    } 

    IEnumerator ShootWeapon(NavMeshAgent navMeshAgent, EnemyWeaponInteraction enemyWeaponInteraction)
    {
        if (enemyWeaponInteraction.currentWeapon == null)
        {
            Debug.LogError("Enemy Doesnt Have Gun");
            yield break;
        }

        isAttacking = true;
        onAttack?.Invoke();
        navMeshAgent.SetDestination(transform.position);

        WeaponShoot weaponShoot = enemyWeaponInteraction.currentWeapon.GetComponent<WeaponShoot>();
        Ammo ammo = enemyWeaponInteraction.currentWeapon.GetComponent<Ammo>();

        Debug.Log("Started Attacking");

        yield return new WaitForSeconds(nextBurst);
        while (true) 
        {         
            for (int i = 0; i < burst; i++)
            {
                if (ammo.ammo == 0) ammo.AddAmmo(ammo.maxAmmo);
                weaponShoot.Shoot(raycastPos, ammo);
                yield return new WaitForSeconds(timeBetweenBullets);
            }

            if (!canAttackTarget) 
            {
                isAttacking = false;
                Debug.Log("Stopped Attacking");
                yield break;
            }

            yield return new WaitForSeconds(nextBurst);
        }    
    }

    void OnDrawGizmos()
    {
        if (path == null) return;

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