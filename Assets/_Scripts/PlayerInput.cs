using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    CameraMove cameraMove;
    Transform mainCamera;

    void Start() 
    {
        player = Player.Instance;
        cameraMove = GetComponentInChildren<CameraMove>();
        mainCamera = Camera.main.transform;
    } 

    void Update() 
    {
        //Move Camera
        cameraMove.camMove();
        //Run
        player.Sprint.Sprinting(KeyCode.LeftShift, KeyCode.W);
        //Shoot Gun
        if (Input.GetKey(KeyCode.Mouse0))
            player.GetPlayerGun().Shooting(mainCamera);
        //Aim Gun
        player.PlayerAnimationManager.AimingWeaponAnimation(Input.GetKey(KeyCode.Mouse1));
        //Reload Gun
        if (Input.GetKeyDown(KeyCode.R))
            player.GetPlayerGun().ReloadGun.Reloading();
        //Pickup Item or Gun
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.WeaponInteraction.TryToPickupWeapon();
            player.UseAndDropItems.pickupItem();
            player.PlayerInteract.Interacting();
        }
        //Drop Gun
        if (Input.GetKeyDown(KeyCode.G)) player.WeaponInteraction.DropGun();
        //Drop Item
        if (Input.GetKeyDown(KeyCode.Q)) player.UseAndDropItems.DropItem();
        //Use Item
        if (Input.GetKeyDown(KeyCode.F)) player.UseAndDropItems.UseItem();
        //Change Inventory Slot
        if (Input.mouseScrollDelta.y < 0) player.selectItem.ChangeSlot(1);
        if (Input.mouseScrollDelta.y > 0) player.selectItem.ChangeSlot(-1);
    }

    void FixedUpdate() 
    {
        //Move Player
        player.Movement.Move(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        //Move Body
        cameraMove.bodyRot();
    }
}
