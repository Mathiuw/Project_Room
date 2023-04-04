using UnityEngine;

public class WeaponParticlesManager : MonoBehaviour
{
    SOWeapon weaponSO;
    WeaponShoot weaponShoot;
    ParticleSystem muzzleFlash;

    void Start()
    {
        weaponSO = GetComponent<Weapon>().weaponSO;
        weaponShoot = GetComponent<WeaponShoot>();
        muzzleFlash = GetComponentInChildren<ParticleSystem>();

        weaponShoot.onShoot += MuzzleFlashPlay;
        weaponShoot.onShoot += WallHitPlay;
    }

    void MuzzleFlashPlay() 
    {    
        muzzleFlash.Play(); 
    }

    void WallHitPlay() 
    {
        if (weaponShoot.hit.transform == null) return;
        if (!weaponShoot.hit.transform.gameObject.isStatic) return;

        GameObject particle = Instantiate(weaponSO.wallHit);
        particle.transform.position = weaponShoot.hit.point;
        particle.transform.forward = weaponShoot.hit.normal;
    }

}
