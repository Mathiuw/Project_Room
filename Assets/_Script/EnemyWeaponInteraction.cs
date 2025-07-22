using System.Collections;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    private void Start()
    {
        if (Weapon && !Weapon.owner)
        {
            StartCoroutine(PickUpWeapon(Weapon));
        }
    }

    public override IEnumerator PickUpWeapon(Weapon weapon)
    {
        Weapon = weapon;

        // Set weapon transform in the weapon container
        weapon.transform.SetParent(weaponContainer, false);
        weapon.transform.position = Vector3.zero;
        weapon.transform.rotation = Quaternion.Euler(Vector3.zero);
        weapon.transform.localScale = Vector3.one;

        weapon.SetHoldState(true, transform);

        Debug.Log(transform.name + " picked up gun");

        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!Weapon) yield break;

        Weapon.AddAmmo(Weapon.SOWeapon.maxAmmo);
        
        yield break;
    }

    public override void DropWeapon()
    {
        //Destroy components
        //Destroy(Weapon.GetComponent<Weapon>());

        //set Ammo drop for player
        //Item ammoDrop = Weapon.AddComponent<Item>();
        //ammoDrop.SOItem = weapon.GetReloadItem();
        //ammoDrop.Amount = 1;
        // Ammo drop pickup
        //Weapon.AddComponent<EnemyAmmoDrop>();

        //Sets weapon state and current weapon NULL
        Weapon.SetHoldState(false, null);
        Weapon = null;

        Debug.Log(name + " dropped weapon");
    }
}
