using UnityEngine;

public class WeaponProjectile : Weapon
{
    [Header("Projectile settings")]
    [SerializeField] private int projectileAmount = 8;

    public override void Shoot(Transform raycastPos)
    {
        
    }
}