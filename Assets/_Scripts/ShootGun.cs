using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;

[RequireComponent(typeof(WeaponAnimations))]
[RequireComponent(typeof(ReloadGun))]
public class ShootGun : MonoBehaviour
{
    [SerializeField] LayerMask shootLayer;
    [SerializeField] ParticleSystem muzzleFlash;

    weapon weapon;
    AudioSource gunSound;
    public ReloadGun ReloadGun { get; private set; }

    [Header("Weapon config")]
    [SerializeField] int damage;
    [SerializeField] float bulletForce = 1f;
    [SerializeField] int bulletMaxDistace = 10000;
    [SerializeField] float fireRate;
    public int ammo;
    public int maximumAmmo;
    float nextTimeToFire = 0;

    public event Action onShoot;
    public event Action<Health> onHit;

    void Start() 
    {
        weapon = GetComponent<weapon>();
        ReloadGun = GetComponent<ReloadGun>();
        gunSound = GetComponent<AudioSource>();

        ammo = maximumAmmo; 
    }

    //Adiciona Munição
    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maximumAmmo) ammo = maximumAmmo;
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

    public void Shooting(Transform raycastPos)
    {
        if (ReloadGun.isReloading) return;
        if (ammo == 0) return;

        if (Time.time > +nextTimeToFire)
        {
            RaycastHit hit;
            Health health;

            onShoot?.Invoke();           
            GunEffects();
            RemoveAmmo(1);

            if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, bulletMaxDistace, shootLayer))
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
