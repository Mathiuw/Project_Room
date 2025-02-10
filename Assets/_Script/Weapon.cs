using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable
{
    [Header("Scriptable object")]
    [SerializeField] SOWeapon soWeapon;

    [Header("Weapon location")]
    [SerializeField] Transform aimLocation;
    [SerializeField] Transform muzzleFlashLocation;
    [SerializeField] Transform ammoMeshTransform;

    AudioSource gunSound;
    public RaycastHit hit;
    float nextTimeToFire = 0;
    int ammo;

    public event Action onShoot;
    public event Action<Health> onHit;

    public Transform holder { get; private set; }

    public SOWeapon GetSOWeapon() { return soWeapon; }

    public int GetAmmo() { return ammo; }

    public int GetMaxAmmo() { return soWeapon.maxAmmo; }

    public Sprite GetAmmoSprite() { return soWeapon.ammoSprite; }

    public float GetFirerate() { return soWeapon.firerate; }

    public float GetReloadTime() { return soWeapon.reloadTime; }

    public SOItem GetReloadItem() { return soWeapon.reloadItem; }

    public float GetIntensity() { return soWeapon.intensity; }

    public float GetSpeed() { return soWeapon.speed; }

    public Vector3 GetAimLocation() { return aimLocation.localPosition; }

    public Vector3 GetMuzzleFlashLocation() { return muzzleFlashLocation.localPosition; }

    public Transform GetAmmoMeshTransform() { return ammoMeshTransform; }

    void Awake()
    {
        // Set hold state to false
        SetHoldState(false);

        // Set ammo to max
        AddAmmo(soWeapon.maxAmmo);

        // Set name to show on hud
        GetComponent<ShowNameToHUD>().SetText(soWeapon.weaponName);

        gunSound = GetComponent<AudioSource>();
    }

    public void Interact(Transform interactor)
    {
        interactor.TryGetComponent(out WeaponInteraction weaponInteraction);

        if (weaponInteraction)
        {
            weaponInteraction.StartCoroutine(weaponInteraction.PickUpWeapon(this));
        }
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        ammo = Mathf.Clamp(ammo, 0, soWeapon.maxAmmo);
    }

    public void RemoveAmmo(int amount)
    {
        ammo -= amount;
        ammo = Mathf.Clamp(ammo, 0, soWeapon.maxAmmo);
    }
 
    public void SetHoldState(bool state, Transform holder = null) 
    {
        this.holder = holder;

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = state;
        if (state) rb.interpolation = RigidbodyInterpolation.None;
        else rb.interpolation = RigidbodyInterpolation.Interpolate;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = state;
    }

    // Shooting logic
    public void Shoot(Transform raycastPos)
    {
        if (ammo == 0) return;
        if (!(Time.time > nextTimeToFire)) return;

        // Firerate calculation
        nextTimeToFire = Time.time + (1f / soWeapon.firerate);
        PlayGunSound();
        PlayMuzzleFlashParticle();
        RemoveAmmo(1);

        //CameraShake.AddCameraShake(soWeapon.intensity, soWeapon.speed);

        // Raycast para checar se atingi algo
        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, soWeapon.shootMask))
        {
            hit.transform.TryGetComponent(out Health health);
            hit.transform.TryGetComponent(out EnemyAi enemyAi);

            // Se atingir um inimigo, vc vira o alvo
            if (enemyAi && holder != null)
            {
                enemyAi.SetTarget(holder);
            }

            // Tira vida do alvo
            if (health)
            {
                onHit?.Invoke(health);
                PlayBloodParticle();
                health.RemoveHealth(soWeapon.damage);

                if (health.GetIsDead()) AddForceToRbs(hit.transform, raycastPos, soWeapon.bulletForce);
            }
            else
            {
                // Adiciona forca ao rigidbody se for um objeto imovel
                AddForceToRbs(hit.transform, raycastPos, soWeapon.bulletForce);
                //PlayWallHitParticle();
            }

            Debug.DrawLine(raycastPos.position, hit.point, Color.green, 1f);    
        }
        else 
        {
            Debug.DrawRay(raycastPos.position, raycastPos.forward, Color.red, 1f);
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
    void AddForceToRbs(Transform hitTransform, Transform directionForce, float forceAmount)
    {
        hitTransform.TryGetComponent(out Rigidbody rb);

        if (rb) 
        {
            rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
        }                         
    }

    // Toca a particula de atirar a arma
    void PlayMuzzleFlashParticle()
    {
        TryGetComponent(out ParticleSystem particleSystem);

        if (particleSystem)
        {
            particleSystem.Play();
        }
    }

    // Toca a particula de atingir a parede
    void PlayWallHitParticle()
    {
        if (hit.transform == null) return;
        if (!hit.transform.gameObject.isStatic) return;

        GameObject particle = Instantiate(Resources.Load("Particle_WallHit") as GameObject);
        particle.transform.position = hit.point;
        particle.transform.forward = hit.normal;
    }

    // Toca a particula de sangue
    void PlayBloodParticle()
    {
        GameObject particle = Instantiate(Resources.Load("Particle_Blood") as GameObject);
        particle.transform.position = hit.point;
        particle.transform.forward = hit.normal;
    }
}