using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    public override IEnumerator PickUpWeapon(Weapon weapon)
    {
        this.Weapon = weapon;

        weapon.transform.SetParent(weaponContainer);
        weapon.transform.transform.localScale = Vector3.one;
        weapon.SetHoldState(true, transform);

        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> picked up gun");

        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!Weapon) yield break;

        Weapon.AddAmmo(Weapon.Ammo);
        
        yield break;
    }

    public override void DropWeapon()
    {
        //Destroy components
        Destroy(Weapon.GetComponent<Weapon>());

        //set Ammo drop for player
        Item ammoDrop = Weapon.AddComponent<Item>();
        //ammoDrop.SOItem = weapon.GetReloadItem();
        ammoDrop.Amount = 1;
        //Ammo drop pickup
        Weapon.AddComponent<EnemyAmmoDrop>();

        //Sets weapon state and current weapon NULL
        Weapon.transform.SetParent(null);
        Weapon.SetHoldState(false, null);
        Weapon = null;

        Debug.Log(name + " dropped weapon");
    }
}
