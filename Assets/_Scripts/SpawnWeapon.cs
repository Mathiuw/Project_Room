using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{
    [SerializeField] SOWeapon weapon;
    bool isMeshSpawned = false;

    void Start() => Spawn();

    void Spawn() 
    {
        SpawnMesh();
        SetWeaponComponents();
        Destroy(this);
    }

    public void SpawnMesh() 
    {
        if (weapon == null || isMeshSpawned)
        {
            Debug.LogError("Weapon is Null or Already Spawned");
            return;
        }

        GameObject model = Instantiate(weapon.Model, transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        isMeshSpawned= true;
    }

    void SetWeaponComponents() 
    {
        if (weapon == null) 
        {
            Debug.LogError("Weapon is Null");
            return;
        } 

        name = weapon.weaponName;

        Name weaponName = gameObject.AddComponent<Name>();
        weaponName.SetText(weapon.weaponName);

        Animator animator = GetComponentInChildren<Animator>();
        if (weapon.animatorOverride != null) animator.runtimeAnimatorController= weapon.animatorOverride;

        GameObject muzzleFlash = Instantiate(weapon.muzzleFlash, transform);
        muzzleFlash.transform.localPosition= weapon.muzzleFlash.transform.position;
        muzzleFlash.transform.localRotation = weapon.muzzleFlash.transform.rotation;
        ParticleSystem MuzzleFlashParticle = muzzleFlash.GetComponent<ParticleSystem>();

        ShootGun shootGun = gameObject.AddComponent<ShootGun>();
        shootGun.SetAttributes(weapon.damage, weapon.firerate, weapon.bulletForce, weapon.maxAmmo, weapon.aimLocation, weapon.shootLayer, MuzzleFlashParticle);

        ReloadGun reloadGun = gameObject.AddComponent<ReloadGun>();
        reloadGun.SetAttributes(weapon.reloadTime, weapon.reloadItem, shootGun);

        GetComponent<AudioSource>().clip = weapon.shootAudio;

        weapon weaponComponent = gameObject.AddComponent<weapon>();

        WeaponAnimationManager weaponAnimationManager = gameObject.AddComponent<WeaponAnimationManager>();
        weaponAnimationManager.SetAttributes(shootGun, animator, weaponComponent);
    }
}
