using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    void Update() 
    {
        //Shoot Gun
        if (Input.GetKey(KeyCode.Mouse0) && Player.Instance.WeaponPickup.IsholdingWeapon())
            Player.Instance.GetPlayerGun().Shooting(Player.Instance.PlayerCamera.transform);
        //Aim Gun
        Player.Instance.AnimationStateController.AimingWeaponAnimation(Input.GetKey(KeyCode.Mouse1));
        //Reload Gun
        if (Input.GetKeyDown(KeyCode.R) && Player.Instance.WeaponPickup.IsholdingWeapon())
            Player.Instance.GetPlayerGun().ReloadGun.Reloading();
        //Pickup Item or Gun
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.Instance.WeaponPickup.PickupRaycast();
            Player.Instance.UseAndDropItems.pickupItem();
        }
        //Drop Gun
        if (Input.GetKeyDown(KeyCode.G)) Player.Instance.WeaponPickup.DropGun(Player.Instance.GetPlayerGun().transform);
        //Drop Item
        if (Input.GetKeyDown(KeyCode.Q)) Player.Instance.UseAndDropItems.DropItem();
        //Use Item
        if (Input.GetKeyDown(KeyCode.F)) Player.Instance.UseAndDropItems.UseItem();
        //Change Inventory Slot
        if (Input.mouseScrollDelta.y < 0) Player.Instance.selectItem.ChangeSlot(1);
        if (Input.mouseScrollDelta.y > 0) Player.Instance.selectItem.ChangeSlot(-1);
    }
}
