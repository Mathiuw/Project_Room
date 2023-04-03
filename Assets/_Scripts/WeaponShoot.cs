using System;
using System.Collections;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    float nextTimeToFire = 0;

    Weapon weapon;
    SOWeapon weaponSO;
    AudioSource gunSound;
    WeaponReload reloadGun;
    Ammo ammo;
    ParticleSystem muzzleFlash;
    public RaycastHit hit;

    public event Action onShoot;
    public event Action<Health> onHit;

    IEnumerator Start() 
    {
        yield return new WaitForEndOfFrame();

        weapon = GetComponent<Weapon>();
        weaponSO = weapon.weaponSO;
        gunSound = GetComponent<AudioSource>();
        reloadGun = GetComponent<WeaponReload>();
        ammo = GetComponent<Ammo>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    void AddForceToRbs(Transform t, Transform directionForce, float forceAmount)
    {
        Rigidbody rb;

        if (t.TryGetComponent(out rb) && !rb.isKinematic) rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
    }

    void GunEffects()
    {
        muzzleFlash.Play(true);
        gunSound.Play();
    }

    bool AutoShoot() { return Input.GetKey(KeyCode.Mouse0); }

    bool SingleShoot() { return Input.GetKeyDown(KeyCode.Mouse0); }

    public void InputShoot(Transform raycastPos)
    {
        switch (weaponSO.shootType)
        {
            case SOWeapon.ShootType.Single: if (SingleShoot()) { Shoot(raycastPos); }
                break;
            case SOWeapon.ShootType.Automatic: if (AutoShoot()) { Shoot(raycastPos); }
                break;
        }
    }

    public void Shoot(Transform raycastPos)
    {
        if (reloadGun.isReloading) return;
        if (ammo.ammo == 0) return;
        if (!(Time.time > nextTimeToFire)) return;

        nextTimeToFire = Time.time + (1f / weaponSO.firerate);
        GunEffects();
        ammo.RemoveAmmo(1);
        onShoot?.Invoke();

        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, weaponSO.shootLayer))
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
    }
}
