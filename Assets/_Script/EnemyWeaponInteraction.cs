using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction
{
    public override IEnumerator PickUpWeapon(Weapon weapon)
    {
        this.weapon = weapon;

        weapon.transform.SetParent(weaponHolder);
        weapon.transform.transform.localScale = Vector3.one;
        weapon.SetHoldState(true, transform);
        
        isHoldingWeapon = true;

        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> picked up gun");

        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!isHoldingWeapon) yield break;

        weapon.AddAmmo(weapon.GetMaxAmmo());
        
        yield break;
    }

    public override void DropWeapon()
    {
        //Destroy components
        Destroy(weapon.GetComponent<Weapon>());

        //set Ammo drop for player
        Item ammoDrop = weapon.AddComponent<Item>();
        ammoDrop.SOItem = weapon.GetReloadItem();
        ammoDrop.amount = 1;
        //Ammo drop pickup
        weapon.AddComponent<EnemyAmmoDrop>();
        //Ammo drop name
        ShowNameToHUD nameComponent = weapon.GetComponent<ShowNameToHUD>();
        nameComponent.SetText("Pickup ammo");

        //Sets weapon state and current weapon NULL
        weapon.transform.SetParent(null);
        weapon.SetHoldState(false, null);
        weapon = null;

        Debug.Log(name + " dropped weapon");
    }
}
