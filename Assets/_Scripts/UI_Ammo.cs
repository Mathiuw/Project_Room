using TMPro;
using UnityEngine;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoUI;
    PlayerWeaponInteraction playerWeaponInteraction;
    Ammo ammo;

    void Start() 
    {
        if (Player.instance != null) 
        {
            playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();

            playerWeaponInteraction.onPickupEnd += ActivateUISprite;
            playerWeaponInteraction.onPickupEnd += AddWeaponEvents;
            playerWeaponInteraction.onDrop += DropUISprite;
            playerWeaponInteraction.onDrop += RemoveWeaponEvents;
        }
        CheckUISprite(playerWeaponInteraction);
    }

    void ActivateUISprite(Transform weapon) 
    {
        ammo = weapon.GetComponent<Ammo>();
        ammoUI.enabled = true;
        SetUIAmmo();
    }

    void DisableUISprite() 
    {
        ammo = null;
        ammoUI.enabled = false;
    }

    void DropUISprite(Transform weapon) => DisableUISprite();

    void CheckUISprite(PlayerWeaponInteraction playerWeaponInteraction) 
    {     
        if (playerWeaponInteraction.isHoldingWeapon) 
        {
            Transform weapon = playerWeaponInteraction.currentWeapon.transform;

            ActivateUISprite(weapon);
        } 
        else DisableUISprite();
    }

    void SetUIAmmo() => ammoUI.SetText(ammo.ammo + "/" + ammo.maxAmmo);

    void AddWeaponEvents(Transform weapon) 
    {
        weapon.GetComponent<WeaponShoot>().onShoot += SetUIAmmo;
        weapon.GetComponent<WeaponReload>().onReloadEnd += SetUIAmmo;
    }

    void RemoveWeaponEvents(Transform weapon) 
    {
        weapon.GetComponent<WeaponShoot>().onShoot -= SetUIAmmo;
        weapon.GetComponent<WeaponReload>().onReloadEnd -= SetUIAmmo;
    } 
}
