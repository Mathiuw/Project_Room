using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int ammo { get; private set; } 
    public int maxAmmo { get; private set; }

    SOWeapon weaponSO;

    void Awake() 
    {
        weaponSO = GetComponent<Weapon>().weaponSO;
        maxAmmo = weaponSO.maxAmmo;
        AddAmmo(maxAmmo); 
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maxAmmo) ammo = maxAmmo;
    }

    public void RemoveAmmo(int amount) => ammo -= amount;
}
