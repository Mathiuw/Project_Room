using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Name))]
[RequireComponent(typeof(WeaponAnimationManager))]
public class Weapon : MonoBehaviour
{
    [Header("Scriptable object")]
    [SerializeField] SOWeapon weaponSO;

    [Header("Weapon settings")]
    [SerializeField] string weaponName;
    [SerializeField] int damage;
    int ammo;
    [SerializeField] int maxAmmo;
    [SerializeField] float bulletForce;
    [SerializeField] float firerate;

    [Header("Crosshair")]
    [SerializeField] public GameObject Crosshair;

    [Header("Camera Shake")]
    [SerializeField] float intensity;
    [SerializeField] float speed;

    [Header("Animations")]
    [SerializeField] AnimatorOverrideController animatorOverride;

    [Header("Reload")]
    [SerializeField] float reloadTime;
    [SerializeField] SOItem reloadItem;

    [Header("Particles")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem wallHit;
    [SerializeField] ParticleSystem Blood;

    [Header("Audio")]
    [SerializeField] AudioClip shootAudio;
    [SerializeField] AudioSource shootSouce;
    [SerializeField] AudioClip noAmmoAudio;
    [SerializeField] AudioSource noAmmoSource;

    [Header("Sprites")]
    [SerializeField] Sprite ammoSprite;

    [Header("Weapon location")]
    [SerializeField] Transform aimLocation;
    [SerializeField] Transform muzzleFlashLocation;
    [SerializeField] Transform ammoMeshTransform;

    [Header("Weapon shoot mode")]
    public ShootType shootType;
    public enum ShootType { Single, Automatic, }
    LayerMask shootMask;

    AudioSource gunSound;
    public RaycastHit hit;

    //EVENTS
    public event Action onShoot;
    public event Action<Health> onHit;

    float nextTimeToFire = 0;

    public Transform holder { get; private set; }

    void Awake()
    {
        name = weaponName;

        GetComponent<Name>().SetText(weaponName);

        SetHoldState(false, null);

        gunSound = GetComponent<AudioSource>();
        AddAmmo(maxAmmo);
    }

    //GETTERS
    public int GetAmmo() { return ammo; }
    public int GetMaxAmmo() { return maxAmmo; }
    public float GetFirerate() { return firerate; }
    public float GetReloadTime() { return reloadTime; }
    public SOItem GetReloadItem() { return reloadItem; }
    public float GetIntensity() { return intensity; }
    public float GetSpeed() { return speed; }
    public Vector3 GetAimLocation() { return aimLocation.localPosition; }
    public Vector3 GetMuzzleFlashLocation() { return muzzleFlashLocation.localPosition; }
    public Transform GetAmmoMeshTransform() { return ammoMeshTransform; }
    public Sprite GetAmmoSprite() { return ammoSprite; }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        ammo = Mathf.Clamp(ammo, 0, maxAmmo);
    }

    public void RemoveAmmo(int amount)
    {
        ammo -= amount;
        ammo = Mathf.Clamp(ammo, 0, maxAmmo);
    }

    public void SetHoldState(bool state, Transform holder) 
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = state;

        this.holder = holder;

        if (state) rb.interpolation = RigidbodyInterpolation.None;
        else rb.interpolation = RigidbodyInterpolation.Interpolate;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = state;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++) 
        {
            if (state) renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            else renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    public void Shoot(Transform raycastPos)
    {
        if (ammo == 0) return;
        if (!(Time.time > nextTimeToFire)) return;

        nextTimeToFire = Time.time + (1f / firerate);
        PlayGunSound();
        PlayMuzzleFlashParticle();
        RemoveAmmo(1);

        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, shootMask))
        {
            Health health;
            EnemyAi enemyAi;

            if ((enemyAi = hit.transform.GetComponentInParent<EnemyAi>()) && holder != null)
            {
                enemyAi.SetTarget(holder);
            }

            if (health = hit.transform.GetComponentInParent<Health>())
            {
                onHit?.Invoke(health);
                PlayBloodParticle();
                health.RemoveHealth(damage);

                if (health.isDead) AddForceToRbs(hit.transform, raycastPos, bulletForce);
            }
            else 
            {
                AddForceToRbs(hit.transform, raycastPos, bulletForce);

                PlayWallHitParticle();
            } 

        }

        onShoot?.Invoke();
    }

    void PlayGunSound()
    {
        gunSound.Play();
    }

    void AddForceToRbs(Transform t, Transform directionForce, float forceAmount)
    {
        Rigidbody rb;

        if (t.TryGetComponent(out rb) && !rb.isKinematic) rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
    }

    void PlayMuzzleFlashParticle()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play();
    }

    void PlayWallHitParticle()
    {
        if (hit.transform == null) return;
        if (!hit.transform.gameObject.isStatic) return;

        GameObject particle = Instantiate(wallHit.gameObject);
        particle.transform.position = hit.point;
        particle.transform.forward = hit.normal;
    }

    void PlayBloodParticle()
    {
        GameObject particle = Instantiate(Blood.gameObject);
        particle.transform.position = hit.point;
        particle.transform.forward = hit.normal;
    }


    void OnDrawGizmos()
    {
        if (hit.transform == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, hit.point);
    }
}