using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyAi : MonoBehaviour
{
    [Header("AI settings")]
    float loopTime = 0.1f;

    [Header("Attack")]
    [SerializeField] Transform raycastPos;
    [SerializeField] float runningSpeedMultiplier;
    [SerializeField] int burst;
    [SerializeField] float nextBurst;

    [Header("Patroling")]
    [SerializeField] Transform path;
    Vector3[] waypoints;
    public Transform target { get; private set; }
    int waypointIndex = 0;

    [Header("Field of view")]
    public float radius;
    [Range(0, 360)] public float angle;
    [SerializeField] LayerMask targetMask, obstructionMask;

    //Booleans de comportamento
    public bool isPatrolling { get; private set; }
    public bool isChasing { get; private set; }
    public bool isAttacking { get; private set; }
    public bool isRunning { get; private set; }
    public bool canSeeTarget { get; private set; }
    public bool canAttackTarget { get; private set; }

    //Eventos
    public event Action onPatrol;
    public event Action onChase;
    public event Action onStopChase;
    public event Action onAttack;
    public event Action onStopAttack;

    EnemyWeaponInteraction enemyWeaponInteraction;
    NavMeshAgent navMeshAgent;
    Collider[] rangeCheck;

    void OnDestroy() => StopAllCoroutines();

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyWeaponInteraction = GetComponent<EnemyWeaponInteraction>();
    }

    void Start()
    {
        waypoints = new Vector3[path.childCount];

        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
    }

    void Update()
    {
        StartCoroutine(FieldOfViewCheck());
    }

    public Transform GetPath() { return path; }

    public float GetSpeedMultiplier() { return runningSpeedMultiplier; }

    public void SetTarget(Transform target = null)
    {
        if (target == null)
        {
            this.target = null;
            canSeeTarget = false;
            canAttackTarget = false;
            return;
        }

        canSeeTarget = true;
        this.target = target;
    }

    bool TargetHasHealth(Transform target) 
    {
        if (target.GetComponent<Health>()) return true;
        else if (target.GetComponentInParent<Health>()) return true;
        else return false;
    }

    IEnumerator FieldOfViewCheck()
    {
        while (true) 
        {
            canAttackTarget = false;

            rangeCheck = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeCheck.Length == 0) yield return new WaitForSeconds(loopTime);

            foreach (Collider collider in rangeCheck)
            {
                Transform target = collider.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        if (this.target == null && target != transform && TargetHasHealth(target)) SetTarget(target);

                        if (distanceToTarget < radius / 1.1f) canAttackTarget = true;
                        else canAttackTarget = false;

                        break;
                    }
                }
                else canAttackTarget = false;
            }

            //Behavior check
            Behavior();

            yield return new WaitForSeconds(loopTime);

        }
    }

    //Comportamento do inimigo
    void Behavior()
    {
        if (!canSeeTarget && !canAttackTarget && !isPatrolling && target == null) StartCoroutine(BehaviorPatrol());
        if (canSeeTarget && !canAttackTarget) 
        {
            if(!isChasing) BehaviorChase();
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.position);
        } 
        if (canSeeTarget && canAttackTarget) 
        {
            navMeshAgent.isStopped = true;
            if (!isAttacking) BehaviorAttack();
        } 
    }

    IEnumerator BehaviorPatrol()
    {
        isPatrolling= true;
        Debug.Log("<color=magenta><b>" + transform.name + "</b></color><color=green> started patrolling</color>");
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
                    Debug.Log("<color=magenta><b>" + transform.name + " </b></color><color=red> stopped patrolling</color>");
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

    void Run() 
    {   
        float oldSpeed = navMeshAgent.speed;

        navMeshAgent.speed *= runningSpeedMultiplier;
        isRunning = true;
        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> started running" +
            " | Old speed = <b><color=red>" + oldSpeed + "</color></b>" +
            " | New speed = <b><color=green>" + navMeshAgent.speed + "</color></b>");
    }
    void BehaviorChase()
    {
        isChasing = true;
        isAttacking = false;
        if (!isRunning) Run();
        onChase?.Invoke();
    }

    void BehaviorAttack() 
    {
        onStopChase?.Invoke();
        isChasing = false;
        isAttacking = true;
        onAttack?.Invoke();
        StartCoroutine(ShootWeapon());
    }

    public IEnumerator ShootWeapon()
    {
        if (enemyWeaponInteraction.Weapon == null)
        {
            Debug.LogError("<b><color=red>Enemy Doesnt Have Gun</color></b>");
            yield break;
        }

        //Pega os scripts
        Debug.Log("<b><color=magenta>" + transform.name + "</color></b><color=green> started attacking </color>");

        yield return new WaitForSeconds(nextBurst);

        while (true) 
        {         
            for (int i = 0; i < burst; i++)
            {
                //Checa se consegue atacar
                if (!canAttackTarget)
                {
                    isAttacking = false;
                    onStopAttack?.Invoke();
                    Debug.Log("<b><color=magenta>" + transform.name + "</color></b> <color=red> stopped attacking </color>");
                    yield break;
                }

                //Se estiver sem munição recarrega
                StartCoroutine(enemyWeaponInteraction.ReloadWeapon());
                //Shoot Weapon
                enemyWeaponInteraction.Weapon.Shoot(raycastPos);

                yield return new WaitForSeconds(1f / enemyWeaponInteraction.Weapon.SOWeapon.firerate);
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

        Gizmos.color = Color.green;

        if (target != null) Gizmos.DrawLine(transform.position, target.position);
    }
}