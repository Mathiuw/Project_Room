using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{
    [SerializeField] SOWeapon weapon;

    void Start() 
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon is Null");
            return;
        }

        SpawnMesh();
        SetWeaponComponents();
        Destroy(this);
    }

    public void SpawnMesh() 
    {
        GameObject model = Instantiate(weapon.Model, transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;
    }

    void SetWeaponComponents() 
    {
        Name weaponName = gameObject.AddComponent<Name>();
        Animator animator = GetComponentInChildren<Animator>();
        Ammo ammo = gameObject.AddComponent<Ammo>();
        WeaponLocations weaponLocations = GetComponentInChildren<WeaponLocations>();
        Weapon weaponComponent = gameObject.AddComponent<Weapon>();
        WeaponShoot weaponShoot = gameObject.AddComponent<WeaponShoot>();
        ReloadGun reloadGun = gameObject.AddComponent<ReloadGun>();
        WeaponAnimationManager weaponAnimationManager = gameObject.AddComponent<WeaponAnimationManager>();

        //weapon name
        name = weapon.weaponName;
        weaponName.SetText(weapon.weaponName);
        //weapon animator
        if (weapon.animatorOverride != null) animator.runtimeAnimatorController= weapon.animatorOverride;
        //weapon ammo
        ammo.SetAttributes(weapon.maxAmmo);         
        //weapon muzzle flash
        GameObject muzzleFlash = Instantiate(weapon.muzzleFlash, transform);
        muzzleFlash.transform.localPosition= weaponLocations.GetMuzzleFlashLocation();
        muzzleFlash.transform.localRotation = weapon.muzzleFlash.transform.rotation;
        ParticleSystem MuzzleFlashParticle = muzzleFlash.GetComponent<ParticleSystem>();
        //weapon shoot
        weaponShoot.SetAttributes(weapon.damage, weapon.firerate, weapon.bulletForce, weapon.shootType, 
            weaponLocations.GetAimLocation(), weapon.shootLayer, MuzzleFlashParticle, weaponComponent);
        //weapon reload
        reloadGun.SetAttributes(weapon.reloadTime, weapon.reloadItem);
        //weapon audio
        GetComponent<AudioSource>().clip = weapon.shootAudio;
        //weapon animation manager
        weaponAnimationManager.SetAttributes(weaponShoot, animator, weaponComponent);
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.localPosition, transform.localPosition + transform.up * .5f);
        Gizmos.DrawLine(transform.localPosition, transform.localPosition + -transform.right * .5f);
    }
}
