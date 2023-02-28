using System;
using UnityEngine;

[RequireComponent(typeof(ReloadGun))]
public class ShootGun : MonoBehaviour
{
    [SerializeField] LayerMask shootLayer;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Weapon config")]
    [SerializeField] int damage;
    [SerializeField] float bulletForce = 1f;
    [field:SerializeField]public float fireRate { get; private set; }
    public int ammo;
    public int maxAmmo;
    float nextTimeToFire = 0;

    public event Action onShoot;
    public event Action<Health> onHit;

    AudioSource gunSound;
    ReloadGun ReloadGun;

    void Awake() 
    {
        ReloadGun = GetComponent<ReloadGun>();
        gunSound = GetComponent<AudioSource>();
    }

    void Start() 
    {
        ammo = maxAmmo; 
    }

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
            nextTimeToFire = Time.time + (1f / fireRate);
        }
    }
}
