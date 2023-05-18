using System.Collections;
using UnityEngine;

public abstract class WeaponInteraction : MonoBehaviour
{
    [SerializeField] protected Transform weaponHolder;

    public bool isHoldingWeapon { get; protected set; } = false;

    public Weapon currentWeapon { get; protected set; }

    void Start() 
    {
        Weapon weapon;

        if (weaponHolder.childCount != 0 && (weapon = weaponHolder.GetComponentInChildren<Weapon>()))
        {
            StartCoroutine(PickUpWeapon(weapon.transform));
        } 
    }

    protected abstract IEnumerator PickUpWeapon(Transform gun);

    public abstract IEnumerator ReloadWeapon(); 

    public abstract void DropWeapon();

}