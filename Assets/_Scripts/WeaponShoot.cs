using System;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    int damage;
    float bulletForce;

    public float firerate { get; private set; }
    float nextTimeToFire = 0;

    public SOWeapon.ShootType shootType { get; private set; }

    LayerMask shootLayer;
    Vector3 aimVector;

    AudioSource gunSound;
    ParticleSystem muzzleFlash;

    public event Action onShoot;
    public event Action<Health> onHit;

    public void SetAttributes(int damage, float firerate, float bulletForce, SOWeapon.ShootType shootType, 
        Vector3 aimVector, LayerMask shootLayer, ParticleSystem muzzleFlash, Weapon weapon)
    {
        this.damage = damage;
        this.firerate = firerate;
        this.bulletForce = bulletForce;
        this.shootType = shootType;
        this.aimVector = aimVector;
        this.shootLayer = shootLayer;
        this.muzzleFlash = muzzleFlash;

        weapon.onHoldStateChange += SetPlayerEvents;
    }

    void Start() 
    {
        gunSound = GetComponent<AudioSource>();
    }

    public Vector3 GetAimVector() { return aimVector; }

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

    public void InputShoot(Transform raycastPos, Ammo ammo, bool isReloading)
    {
        if (shootType == SOWeapon.ShootType.Single && SingleShoot()) Shoot(raycastPos, ammo, isReloading);
        else if (shootType == SOWeapon.ShootType.Automatic && AutoShoot()) Shoot(raycastPos, ammo, isReloading);
    }

    public void Shoot(Transform raycastPos, Ammo ammo, bool isReloading = false)
    {
        if (isReloading) return;
        if (ammo.ammo == 0) return;

        if (Time.time > +nextTimeToFire)
        {
            RaycastHit hit;
            Health health;

            GunEffects();
            ammo.RemoveAmmo(1);
            onShoot?.Invoke();

            if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, shootLayer))
            {
                if (health = hit.transform.GetComponentInParent<Health>())
                {
                    onHit?.Invoke(health);
                    health.RemoveHealth(damage);
                    if (health.Isdead()) AddForceToRbs(hit.transform, raycastPos, bulletForce);
                }
                else AddForceToRbs(hit.transform, raycastPos, bulletForce);
            }
            nextTimeToFire = Time.time + (1f / firerate);
        }
    }

    public void SetPlayerEvents(PlayerWeaponInteraction playerWeaponInteraction, bool state) 
    {
        if (playerWeaponInteraction == null) return;

        if (state) playerWeaponInteraction.onShoot += InputShoot;
        else playerWeaponInteraction.onShoot -= InputShoot;
    }
}
