using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoUI;
    [SerializeField] Image ammoSprite;
    PlayerWeaponInteraction playerWeaponInteraction;
    Inventory playerInventory;

    void Start() 
    {        
        playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction) 
        {
            playerWeaponInteraction.onWeaponPickup += ActivateUISprite;
            playerWeaponInteraction.onWeaponPickup += AddWeaponEvents;
            playerWeaponInteraction.onWeaponDrop += DisableUISprite;
            playerWeaponInteraction.onWeaponDrop += RemoveWeaponEvents;
            playerWeaponInteraction.onReloadStart += OnRealoadFunc;
            playerWeaponInteraction.onReloadEnd += SetUIAmmoText;


            CheckUISprite(playerWeaponInteraction);
        }
        else
        {
            Debug.LogError("Can find PlayerWeaponInteraction on player GameObject");
            enabled = false;
            return;
        }

        playerInventory = playerWeaponInteraction.GetComponent<Inventory>();

        if (playerInventory)
        {
            playerInventory.OnAmmoUpdate += SetUIAmmoText;


        }
        else
        {
            Debug.LogError("Cant find Inventory on player GameObject");
            enabled = false;
            return;
        }
    }

    private void OnDisable()
    {
        playerWeaponInteraction.onWeaponPickup -= ActivateUISprite;
        playerWeaponInteraction.onWeaponPickup -= AddWeaponEvents;
        playerWeaponInteraction.onWeaponDrop -= DisableUISprite;
        playerWeaponInteraction.onWeaponDrop -= RemoveWeaponEvents;
        playerWeaponInteraction.onReloadStart -= OnRealoadFunc;
        playerWeaponInteraction.onReloadEnd -= SetUIAmmoText;

        playerInventory.OnAmmoUpdate -= SetUIAmmoText;
    }

    void OnRealoadFunc(float duration) 
    {
        SetUIAmmoText();
    }

    void ActivateUISprite(Weapon weaponPicked = null) 
    {
        ammoUI.enabled = true;
        ammoSprite.sprite = weaponPicked.GetAmmoSprite();
        SetUIAmmoText();
    }

    void DisableUISprite() 
    {
        ammoUI.enabled = false;
        ammoSprite.enabled = false;
    }

    void CheckUISprite(PlayerWeaponInteraction playerWeaponInteraction) 
    {     
        if (playerWeaponInteraction.GetWeapon()) ActivateUISprite();
        else DisableUISprite();
    }

    void SetUIAmmoText() => ammoUI.SetText(playerWeaponInteraction.GetWeapon().GetAmmo() + "/" + playerInventory.GetAmmoAmountByType(playerWeaponInteraction.GetWeapon().GetSOWeapon().ammoType));

    void AddWeaponEvents(Weapon weaponPicked) 
    {
        playerWeaponInteraction.GetWeapon().GetComponent<Weapon>().onShoot += SetUIAmmoText;
    }

    void RemoveWeaponEvents() 
    {
        playerWeaponInteraction.GetWeapon().GetComponent<Weapon>().onShoot -= SetUIAmmoText;
    } 
}
