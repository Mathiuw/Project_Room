using System;
using UnityEngine;

public class ShootGun : MonoBehaviour
{
    int damage;
    float bulletForce;

    public float firerate { get; private set; }
    float nextTimeToFire = 0;

    public int ammo { get; private set; }

    public int maxAmmo { get; private set; }

    LayerMask shootLayer;
    Vector3 aimVector;

    AudioSource gunSound;
    ReloadGun ReloadGun;
    ParticleSystem muzzleFlash;

    public event Action onShoot;
    public event Action<Health> onHit;

    public void SetAttributes(int damage, float firerate, float bulletForce, int maxAmmo, Vector3 aimVector, LayerMask shootLayer, ParticleSystem muzzleFlash)
    {
        this.damage = damage;
        this.firerate = firerate;
        this.bulletForce = bulletForce;
        this.maxAmmo = maxAmmo;
        this.aimVector = aimVector;
        this.shootLayer = shootLayer;
        this.muzzleFlash = muzzleFlash;
    }

    void Start() 
    {
        ReloadGun = GetComponent<ReloadGun>();
        gunSound = GetComponent<AudioSource>();

        AddAmmo(maxAmmo); 
    }

    public Vector3 GetAimVector() { return aimVector; }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maxAmmo) ammo = maxAmmo;
    }

    public void RemoveAmmo(int amount) => ammo -= amount;

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

    public void Shoot(Transform raycastPos)
    {
        if (ReloadGun.isReloading) return;
        if (ammo == 0) return;

        if (Time.time > +nextTimeToFire)
        {
            RaycastHit hit;
            Health health;

            GunEffects();
            RemoveAmmo(1);
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
}
