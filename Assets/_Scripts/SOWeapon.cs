using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class SOWeapon : ScriptableObject
{
    [Header("Weapon")]
    public string weaponName;
    public int damage;
    public float bulletForce;
    public int maxAmmo;
    public float firerate;
    public bool waitToShoot;
    public Weapon.ShootType shootType;
    public LayerMask shootMask;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Camera Shake")]
    public float intensity;
    public float speed;

    [Header("Animations")]
    public AnimatorOverrideController animatorOverride;

    [Header("Reload")]
    public float reloadTime;
    public SOItem reloadItem;

    [Header("Particles")]
    public GameObject muzzleFlash;
    public GameObject wallHit;
    public GameObject Blood;

    [Header("UI Sprite")]
    public Sprite ammoSprite;

    //[Header("Audio")]
    //public AudioClip shootAudio;
    //public AudioSource shootSouce;
    //public AudioClip noAmmoAudio;
    //public AudioSource noAmmoSource;
}