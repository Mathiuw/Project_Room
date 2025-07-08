using System.Collections;
using UnityEngine;

public abstract class WeaponInteraction : MonoBehaviour
{
    [Header("Weapon settings")]
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Weapon weapon;

    public Weapon GetWeapon() { return weapon; }

    public abstract IEnumerator PickUpWeapon(Weapon weapon);

    public abstract IEnumerator ReloadWeapon(); 

    public abstract void DropWeapon();

}