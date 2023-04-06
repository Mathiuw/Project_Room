using TMPro;
using UnityEngine;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoUI;
    PlayerWeaponInteraction playerWeaponInteraction;
    WeaponAmmo ammo;

    void Start() 
    {
        if (Player.instance != null) 
        {
            playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();

            playerWeaponInteraction.onPickupEnd += ActivateUISprite;
            playerWeaponInteraction.onPickupEnd += AddWeaponEvents;
            playerWeaponInteraction.onDrop += DisableUISprite;
            playerWeaponInteraction.onDrop += RemoveWeaponEvents;
            playerWeaponInteraction.onReloadStart += SetUIAmmo;
            playerWeaponInteraction.onReloadEnd += SetUIAmmo;
        }
        CheckUISprite(playerWeaponInteraction);
    }

    void ActivateUISprite() 
    {
        ammo = playerWeaponInteraction.currentWeapon.GetComponent<WeaponAmmo>();
        ammoUI.enabled = true;
        SetUIAmmo();
    }

    void DisableUISprite() 
    {
        ammo = null;
        ammoUI.enabled = false;
    }

    void CheckUISprite(PlayerWeaponInteraction playerWeaponInteraction) 
    {     
        if (playerWeaponInteraction.isHoldingWeapon) ActivateUISprite();
        else DisableUISprite();
    }

    void SetUIAmmo() => ammoUI.SetText(ammo.ammo + "/" + ammo.maxAmmo);

    void AddWeaponEvents() 
    {
        playerWeaponInteraction.currentWeapon.GetComponent<WeaponShoot>().onShoot += SetUIAmmo;
    }

    void RemoveWeaponEvents() 
    {
        playerWeaponInteraction.currentWeapon.GetComponent<WeaponShoot>().onShoot -= SetUIAmmo;
    } 
}
