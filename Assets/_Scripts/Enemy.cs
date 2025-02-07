using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    void Start() => GetComponentInChildren<Health>().onDead += OnDead;

    void OnDead() 
    {
        WeaponInteraction weaponInteraction = GetComponent<WeaponInteraction>();

        //Dropa a arma
        if (weaponInteraction.GetIsHoldingWeapon()) weaponInteraction.DropWeapon();
        
        //Destroi os componentes
        Destroy(GetComponent<RigBuilder>());
        Destroy(GetComponent<Animator>());
        Destroy(GetComponent<EnemyWeaponInteraction>());
        Destroy(GetComponent<EnemyAnimationManager>());
        Destroy(GetComponent<EnemyAi>().GetPath().gameObject);
        Destroy(GetComponent<EnemyAi>());
        Destroy(GetComponent<DoorDestroyer>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<Rigidbody>());
        //Ativa o ragdoll
        GetComponent<Ragdoll>().RagdollActive(true);
        Destroy(GetComponent<Ragdoll>());
        Destroy(this);
    }
}
