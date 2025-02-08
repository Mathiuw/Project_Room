using System;
using UnityEngine;

[RequireComponent(typeof(ShowNameToHUD))]
[RequireComponent(typeof(WeaponAnimationManager))]
public class Weapon : MonoBehaviour
{
    [Header("Scriptable object")]
    [SerializeField] SOWeapon soWeapon;

    [Header("Weapon settings")]
    [SerializeField] string weaponName;
    [SerializeField] int damage;
    int ammo;
    [SerializeField] int maxAmmo;
    [SerializeField] float bulletForce;
    [SerializeField] float firerate;
    [SerializeField] LayerMask shootMask;

    [Header("Weapon shoot mode")]
    public ShootType shootType;

    [Header("Crosshair")]
    [SerializeField] public GameObject crosshair;

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

    [Header("Sprites")]
    [SerializeField] Sprite ammoSprite;

    [Header("Weapon location")]
    [SerializeField] Transform aimLocation;
    [SerializeField] Transform muzzleFlashLocation;
    [SerializeField] Transform ammoMeshTransform;

    public enum ShootType { Single, Automatic }

    AudioSource gunSound;
    public RaycastHit hit;

    public event Action onShoot;
    public event Action<Health> onHit;

    float nextTimeToFire = 0;

    public Transform holder { get; private set; }

    void Awake()
    {
        SetWeaponStats();

        SetHoldState(false, null);

        gunSound = GetComponent<AudioSource>();
        AddAmmo(maxAmmo);
    }

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

    // Adiciona as informações da arma de acordo com o scriptable object
    void SetWeaponStats() 
    {
        weaponName = soWeapon.weaponName;
        name = weaponName;
        GetComponent<ShowNameToHUD>().SetText(weaponName);
        damage = soWeapon.damage;
        maxAmmo = soWeapon.maxAmmo;
        ammo = maxAmmo;
        bulletForce = soWeapon.bulletForce;
        firerate = soWeapon.firerate;
        shootMask = soWeapon.shootMask;
        shootType = soWeapon.shootType;

        if (soWeapon.crosshair != null)
        {
            crosshair = soWeapon.crosshair;
        }
    
        intensity = soWeapon.intensity;
        speed = soWeapon.speed;
        animatorOverride = soWeapon.animatorOverride;
        reloadTime = soWeapon.reloadTime;
        reloadItem = soWeapon.reloadItem;
        muzzleFlash = soWeapon.muzzleFlash.GetComponent<ParticleSystem>();
        wallHit = soWeapon.wallHit.GetComponent<ParticleSystem>();
        Blood = soWeapon.Blood.GetComponent<ParticleSystem>();
    }

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

    // 
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

    // Shooting logic
    public void Shoot(Transform raycastPos)
    {
        if (ammo == 0) return;
        if (!(Time.time > nextTimeToFire)) return;

        // Firerate calculation
        nextTimeToFire = Time.time + (1f / firerate);
        PlayGunSound();
        PlayMuzzleFlashParticle();
        //CameraShake.AddCameraShake(Camera.main.transform, intensity, speed);
        RemoveAmmo(1);

        // Raycast para checar se atingi algo
        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, shootMask))
        {
            Health health;
            EnemyAi enemyAi;

            // Se atingir um inimigo, vc vira o alvo
            if ((enemyAi = hit.transform.GetComponentInParent<EnemyAi>()) && holder != null)
            {
                enemyAi.SetTarget(holder);
            }

            // Tira vida do alvo
            if (health = hit.transform.GetComponentInParent<Health>())
            {
                onHit?.Invoke(health);
                PlayBloodParticle();
                health.RemoveHealth(damage);

                if (health.GetIsDead()) AddForceToRbs(hit.transform, raycastPos, bulletForce);
            }
            else 
            {
                // Adiciona forca ao rigidbody se for um objeto imovel
                AddForceToRbs(hit.transform, raycastPos, bulletForce);
                PlayWallHitParticle();
            } 

        }

        // Invoca o evento de atirar
        onShoot?.Invoke();
    }

    // Toca som da arma
    void PlayGunSound()
    {
        gunSound.Play();
    }

    // Adiciona forca a Rigidbodys
    void AddForceToRbs(Transform t, Transform directionForce, float forceAmount)
    {
        Rigidbody rb;

        if (t.TryGetComponent(out rb) && !rb.isKinematic) rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
    }

    // Toca a particula de atirar a arma
    void PlayMuzzleFlashParticle()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play();
    }

    // Toca a particula de atingir a parede
    void PlayWallHitParticle()
    {
        if (hit.transform == null) return;
        if (!hit.transform.gameObject.isStatic) return;

        GameObject particle = Instantiate(wallHit.gameObject);
        particle.transform.position = hit.point;
        particle.transform.forward = hit.normal;
    }

    // Toca a particula de sangue
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