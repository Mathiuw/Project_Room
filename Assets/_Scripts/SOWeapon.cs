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
    public ShootType shootType;
    public enum ShootType { Single, Automatic, }
    public LayerMask shootLayer;
    public GameObject Model;
    [Header("Animations")]
    public AnimatorOverrideController animatorOverride;
    [Header("Reload")]
    public float reloadTime;
    public SOItem reloadItem;
    [Header("Muzzle Flash")]
    public GameObject muzzleFlash;
    [Header("Audio")]
    public AudioClip shootAudio;
    public AudioSource shootSouce;
    public AudioClip noAmmoAudio;
    public AudioSource noAmmoSource;
}
