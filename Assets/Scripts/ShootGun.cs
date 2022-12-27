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

    AudioSource gunSound;
    public ReloadGun ReloadGun { get; private set; }

    [Header("Weapon config")]
    [SerializeField] int damage;
    [SerializeField] float bulletForce = 1f;
    [SerializeField] int bulletMaxDistace = 100;
    [SerializeField] float fireRate;
    public int ammo;
    public int maximumAmmo;
    float nextTimeToFire = 0;

    public event Action OnShoot;

    void Start() 
    {
        ReloadGun = GetComponent<ReloadGun>();
        gunSound = GetComponent<AudioSource>();

        ammo = maximumAmmo; 
    }

    //Adicionar Munição
    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maximumAmmo) ammo = maximumAmmo;
    }

    //Remover Munição
    public void RemoveAmmo(int amount) => ammo -= amount;

    //Ação de atirar
    public void Shooting(Transform raycastPos)
    {
        if (ReloadGun.reloading) return;
        if (ammo == 0) return;

        if (Time.time > +nextTimeToFire)
        {
            RaycastHit hit;
            Health health;

            OnShoot?.Invoke();           
            GunEffects();
            RemoveAmmo(1);

            if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, bulletMaxDistace, shootLayer))
            {
                if (health = hit.transform.GetComponentInParent<Health>())
                {
                    health.RemoveHealth(damage);
                    AddForceToDeadBodies(hit.transform, raycastPos, bulletForce,health);
                    Debug.Log(hit.transform.name + " - life = " + health.HealthAmount);
                }
                else Debug.LogError(hit.transform.name + " Doesnt Have Health");            
            }
            nextTimeToFire = Time.time + (1f / fireRate);
        }
    }

    void AddForceToDeadBodies(Transform t, Transform directionForce, float forceAmount, Health health) 
    {
        Rigidbody rb;

        if (!health.Isdead()) 
        {
            Debug.Log(t.name + " is not dead");
            return;
        }
        if (t.TryGetComponent(out rb)) rb.AddForce(directionForce.forward * forceAmount, ForceMode.VelocityChange);
        else Debug.LogError("rbs Not Found");
    }

    //Muzzle Flash e Som da Arma
    void GunEffects()
    {
        muzzleFlash.Play(true);
        gunSound.Play();
    }

    //Reseta os Eventos da Arma
    public void ResetGunEvents() => OnShoot = null;
}
