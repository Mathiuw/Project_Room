using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable, IUIName
{
    [Header("Weapon Scriptable object")]
    [field: SerializeField] public SOWeapon SOWeapon { get; private set; }

    [Header("Weapon Transforms")]
    [SerializeField] Transform muzzleFlashLocation;

    [Header("Particles")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem blood;

    AudioSource gunSound;
    public RaycastHit hit;
    float nextTimeToFire = 0;

    public int Ammo { get; private set; } = 0;

    public event Action onShoot;
    public event Action<Health> onHit;

    public Transform owner { get; private set; }

    public string ReadName => SOWeapon.weaponName;

    public Vector3 GetMuzzleFlashLocation() { return muzzleFlashLocation.localPosition; }

    void Awake()
    {
        // Set hold state to false
        SetHoldState(false);

        // Set ammo to max
        AddAmmo(SOWeapon.maxAmmo);

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
        Ammo += amount;
        Ammo = Mathf.Clamp(Ammo, 0, SOWeapon.maxAmmo);
    }

    public void RemoveAmmo(int amount)
    {
        Ammo -= amount;
        Ammo = Mathf.Clamp(Ammo, 0, SOWeapon.maxAmmo);
    }
 
    public void SetHoldState(bool hasOwner, Transform owner = null) 
    {
        this.owner = owner;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = hasOwner;

        if (hasOwner)
        {
            rb.interpolation = RigidbodyInterpolation.None;
        }
        else 
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            transform.SetParent(null);
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = hasOwner;
    }

    // Shooting logic
    public virtual void Shoot(Transform raycastPos)
    {
        if (Ammo == 0) return;
        if (!(Time.time > nextTimeToFire)) return;

        // Firerate calculation
        nextTimeToFire = Time.time + (1f / SOWeapon.firerate);
        PlayGunSound();
        PlayMuzzleFlashParticle();
        RemoveAmmo(1);

        //CameraShake.AddCameraShake(soWeapon.intensity, soWeapon.speed);

        // Raycast para checar se atingi algo
        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, SOWeapon.shootMask))
        {
            Debug.DrawLine(raycastPos.position, hit.point, Color.green, 1f);

            hit.transform.TryGetComponent(out Health health);
            hit.transform.TryGetComponent(out EnemyAi enemyAi);

            if (enemyAi && owner != null) enemyAi.Target = owner;

            if (health)
            {
                onHit?.Invoke(health);
                PlayBloodParticle();
                health.RemoveHealth(SOWeapon.damage);

                if (health.GetIsDead()) AddForceToRbs(hit.transform, raycastPos, SOWeapon.bulletForce);
            }
            else
            {
                AddForceToRbs(hit.transform, raycastPos, SOWeapon.bulletForce);
            }
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
        Instantiate(muzzleFlash, muzzleFlashLocation.position, muzzleFlashLocation.rotation, transform);
    }

    // Toca a particula de sangue
    void PlayBloodParticle()
    {
        ParticleSystem particleSystem = Instantiate(blood, hit.point, Quaternion.identity, hit.transform);
        particleSystem.transform.forward = hit.normal;
    }
}