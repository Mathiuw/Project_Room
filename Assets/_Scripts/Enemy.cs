using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Awake() => GetComponentInChildren<Die>().onDead += OnDead;

    void OnDead() 
    {
        GetComponent<WeaponInteraction>().DropWeapon();
        Destroy(GetComponentInChildren<Animator>());
        GetComponentInChildren<Ragdoll>().RagdollActive(true);
        GetComponentInChildren<Ragdoll>().transform.SetParent(null);

        Destroy(gameObject);
    }
}
