using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoUI;
    [SerializeField] Image ammoSprite;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Start() 
    {        
        playerWeaponInteraction = FindFirstObjectByType<Player>().GetComponent<PlayerWeaponInteraction>();
        playerWeaponInteraction.onWeaponPickup += ActivateUISprite;
        playerWeaponInteraction.onWeaponPickup += AddWeaponEvents;
        playerWeaponInteraction.onWeaponDrop += DisableUISprite;
        playerWeaponInteraction.onWeaponDrop += RemoveWeaponEvents;
        playerWeaponInteraction.onReloadStart += OnRealoadFunc;
        playerWeaponInteraction.onReloadEnd += SetUIAmmo;

        CheckUISprite(playerWeaponInteraction);
    }

    void OnRealoadFunc(float duration) 
    {
        SetUIAmmo();
    }

    void ActivateUISprite(Weapon weaponPicked = null) 
    {
        ammoUI.enabled = true;
        ammoSprite.sprite = weaponPicked.GetAmmoSprite();
        SetUIAmmo();
    }

    void DisableUISprite() 
    {
        ammoUI.enabled = false;
        ammoSprite.enabled = false;
    }

    void CheckUISprite(PlayerWeaponInteraction playerWeaponInteraction) 
    {     
        if (playerWeaponInteraction.GetIsHoldingWeapon()) ActivateUISprite();
        else DisableUISprite();
    }

    void SetUIAmmo() => ammoUI.SetText(playerWeaponInteraction.GetWeapon().GetAmmo() + "/" + playerWeaponInteraction.GetWeapon().GetMaxAmmo());

    void AddWeaponEvents(Weapon weaponPicked) 
    {
        playerWeaponInteraction.GetWeapon().GetComponent<Weapon>().onShoot += SetUIAmmo;
    }

    void RemoveWeaponEvents() 
    {
        playerWeaponInteraction.GetWeapon().GetComponent<Weapon>().onShoot -= SetUIAmmo;
    } 
}
