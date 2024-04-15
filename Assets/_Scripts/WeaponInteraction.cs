using System.Collections;
using UnityEngine;

public abstract class WeaponInteraction : MonoBehaviour
{
    [SerializeField] protected Transform weaponHolder;

    public bool isHoldingWeapon { get; protected set; } = false;

    [Header("Weapon settings")]
    [SerializeField] protected Weapon weapon;

    public Weapon GetWeapon() { return weapon; }

    protected abstract IEnumerator PickUpWeapon(Weapon weapon);

    public abstract IEnumerator ReloadWeapon(); 

    public abstract void DropWeapon();

}