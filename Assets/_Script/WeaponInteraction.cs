using System.Collections;
using UnityEngine;

public abstract class WeaponInteraction : MonoBehaviour
{
    [Header("Weapon settings")]
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected Weapon weapon;
    protected bool isHoldingWeapon = false;

    public Weapon GetWeapon() { return weapon; }

    public bool GetIsHoldingWeapon() { return isHoldingWeapon; }

    public abstract IEnumerator PickUpWeapon(Weapon weapon);

    public abstract IEnumerator ReloadWeapon(); 

    public abstract void DropWeapon();

}