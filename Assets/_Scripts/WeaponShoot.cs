using System;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    float nextTimeToFire = 0;

    Weapon weapon;
    SOWeapon weaponSO;
    AudioSource gunSound;
    WeaponAmmo ammo;
    public RaycastHit hit;

    public event Action onShoot;
    public event Action<Health> onHit;

    void Start() 
    {
        weapon = GetComponent<Weapon>();
        weaponSO = weapon.weaponSO;
        gunSound = GetComponent<AudioSource>();
        ammo = GetComponent<WeaponAmmo>();
    }

    void AddForceToRbs(Transform t, Transform directionForce, float forceAmount)
    {
        Rigidbody rb;

        if (t.TryGetComponent(out rb) && !rb.isKinematic) rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
    }

    void GunSound()
    { 
        gunSound.Play();
    }

    public void Shoot(Transform raycastPos)
    {
        if (ammo.ammo == 0) return;
        if (!(Time.time > nextTimeToFire)) return;

        nextTimeToFire = Time.time + (1f / weaponSO.firerate);
        GunSound();
        ammo.RemoveAmmo(1);
        
        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, weaponSO.shootMask))
        {
            Health health;
            EnemyAi enemyAi;

            if ((enemyAi = hit.transform.GetComponentInParent<EnemyAi>()) && weapon.holder != null)
            {
                enemyAi.SetTarget(weapon.holder);
            }

            if (health = hit.transform.GetComponentInParent<Health>())
            {
                onHit?.Invoke(health);
                health.RemoveHealth(weaponSO.damage);

                if (health.Isdead()) AddForceToRbs(hit.transform, raycastPos, weaponSO.bulletForce);
            }
            else AddForceToRbs(hit.transform, raycastPos, weaponSO.bulletForce);
        }

        onShoot?.Invoke();
    }

    void OnDrawGizmos()
    {
        if (hit.transform == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, hit.point);
    }
}
