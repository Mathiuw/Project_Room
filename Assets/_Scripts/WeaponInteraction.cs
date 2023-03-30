using System.Collections;
using UnityEngine;

public abstract class WeaponInteraction : MonoBehaviour
{
    [SerializeField] protected Transform gunHolder;

    public bool isHoldingWeapon { get; protected set; } = false;

    public Weapon currentWeapon { get; protected set; }

    IEnumerator Start() 
    {
        yield return new WaitForEndOfFrame();

        Weapon weapon;

        if (gunHolder.childCount != 0 && gunHolder.GetChild(0).TryGetComponent(out weapon)) StartCoroutine(PickUpWeapon(gunHolder.GetChild(0)));
    }

    protected abstract IEnumerator PickUpWeapon(Transform gun);

    public abstract IEnumerator ReloadWeapon(); 

    public abstract void DropWeapon();

}