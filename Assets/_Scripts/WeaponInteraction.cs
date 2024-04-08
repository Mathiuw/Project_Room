using System.Collections;
using UnityEngine;

public abstract class WeaponInteraction : MonoBehaviour
{
    [SerializeField] protected Transform weaponHolder;

    public bool isHoldingWeapon { get; protected set; } = false;

    [Header("Weapon settings")]
    [SerializeField]Weapon weapon;
    public Weapon Weapon { get => weapon; protected set => Weapon = value; }

    protected abstract IEnumerator PickUpWeapon(Transform gun);

    public abstract IEnumerator ReloadWeapon(); 

    public abstract void DropWeapon();

}