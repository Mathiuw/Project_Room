using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

//[RequireComponent(typeof(Health), typeof(EnemyWeaponInteraction), typeof(NavMeshAgent))]
public class EnemyAi : MonoBehaviour
{
    [Header("AI settings")]
    [SerializeField] float _baseSpeed = 6;
    [SerializeField] float _runningSpeedMultiplier = 1.4f;

    [field: Header("Attack")]
    [field: SerializeField] public Transform _shootRaycastTransform;
    [SerializeField] float attackRange = 10f;
    [SerializeField] int _burstCount = 3;
    [SerializeField] float _burstInterval = 1f;

    [Header("Patroling")]
    [SerializeField] Path _path;

    [Header("Field of view")]
    [Range(0, 360)] public float _angle = 160;
    public float Radius { get; } = 20;
    [field: SerializeField] public LayerMask TargetMask { get; private set; }
    [field: SerializeField] public LayerMask ObstructionMask { get; private set; }

    public Transform Target { get; set; }

    EnemyWeaponInteraction _enemyWeaponInteraction;
    NavMeshAgent _navMeshAgent;
    StateMachine _stateMachine;

    void OnDestroy() => StopAllCoroutines();

    void Awake()
    {
        if (!_path)
        {
            Debug.LogError("Enemy doesnt have aptch");
            enabled = false;
            return;
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _baseSpeed;

        _enemyWeaponInteraction = GetComponent<EnemyWeaponInteraction>();

        Health health = GetComponent<Health>();
        health.onDead += OnDead;
    }

    private void Start()
    {
        _stateMachine = new StateMachine();

        Patrolling patrolling = new Patrolling(this, _path, _navMeshAgent);
        Chase chase = new Chase(this, _navMeshAgent);
        Attack attack = new Attack(this, _navMeshAgent);

        At(patrolling, chase, HasTarget());
        At(chase, attack, IsInTargetReach());
        At(attack, chase, IsNotInTargetReach());

        _stateMachine.SetState(patrolling);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> IsInTargetReach() => () => Vector3.Distance(transform.position, Target.position) < attackRange /*&& CanSeeTarget()*/;
        Func<bool> IsNotInTargetReach() => () => Vector3.Distance(transform.position, Target.position) > attackRange;
    }

    void Update()
    {
        _stateMachine?.Tick();
    }

    public void Run(bool value)
    {
        if (value)
        {
            _navMeshAgent.speed = _baseSpeed * _runningSpeedMultiplier;
        }
        else _navMeshAgent.speed = _baseSpeed;
    }

    public bool CanSeeTarget()
    {
        Vector3 directionToTarget = (Target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
        {
            return true;
        }
        else return false;
    }

    public void LookToTarget()
    {
        transform.LookAt(Target);
    }

    public void StartShooting()
    {
        StartCoroutine(ShootWeapon());
    }

    public void StopShooting()
    {
        StopCoroutine(ShootWeapon());
    }

    public IEnumerator ShootWeapon()
    {
        if (_enemyWeaponInteraction.Weapon == null) 
        {
            Debug.Log("Enemy doesnt have weapon");
            yield break;
        } 

        while (true)
        {
            for (int i = 0; i < _burstCount; i++)
            {
                // Reload
                StartCoroutine(_enemyWeaponInteraction.ReloadWeapon());
                // Shoot Weapon
                _enemyWeaponInteraction.Weapon.Shoot(_shootRaycastTransform);

                yield return new WaitForSeconds(1f / _enemyWeaponInteraction.Weapon.SOWeapon.firerate);
            }

            yield return new WaitForSeconds(_burstInterval);
        }
    }

    void OnDead()
    {
        // Drop weapon
        WeaponInteraction weaponInteraction = GetComponent<WeaponInteraction>();
        weaponInteraction?.DropWeapon();

        // Destroy unused components
        Destroy(GetComponent<EnemyWeaponInteraction>());;
        Destroy(GetComponent<DoorDestroyer>());
        Destroy(GetComponent<CapsuleCollider>());

        // Activate ragdoll
        GetComponentInChildren<Ragdoll>()?.RagdollState(true);

        // Destroy self
        Destroy(this);
    }
}