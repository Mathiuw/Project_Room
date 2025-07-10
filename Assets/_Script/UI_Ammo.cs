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
            playerInventory.OnAmmoCountUpdate += SetUIAmmoText;


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

        playerInventory.OnAmmoCountUpdate -= SetUIAmmoText;
    }

    void OnRealoadFunc(float duration) 
    {
        SetUIAmmoText();
    }

    void ActivateUISprite(Weapon weaponPicked = null) 
    {
        ammoUI.enabled = true;
        ammoSprite.sprite = weaponPicked.SOWeapon.ammoSprite;
        SetUIAmmoText();
    }

    void DisableUISprite() 
    {
        ammoUI.enabled = false;
        ammoSprite.enabled = false;
    }

    void CheckUISprite(PlayerWeaponInteraction playerWeaponInteraction) 
    {     
        if (playerWeaponInteraction.Weapon) ActivateUISprite();
        else DisableUISprite();
    }

    void SetUIAmmoText() 
    {
        if (!playerWeaponInteraction.Weapon)
        {
            ammoUI.SetText("");
            return;
        }

        ammoUI.SetText(playerWeaponInteraction.Weapon.Ammo + "/" + playerInventory.GetAmmoAmountByType(playerWeaponInteraction.Weapon.SOWeapon.ammoType));
    } 

    void AddWeaponEvents(Weapon weaponPicked) 
    {
        playerWeaponInteraction.Weapon.GetComponent<Weapon>().onShoot += SetUIAmmoText;
    }

    void RemoveWeaponEvents() 
    {
        playerWeaponInteraction.Weapon.GetComponent<Weapon>().onShoot -= SetUIAmmoText;
    } 
}
