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
            playerWeaponInteraction.onWeaponShot += SetUIAmmoText;
            playerWeaponInteraction.onWeaponDrop += DisableUISprite;

            if (playerWeaponInteraction.Weapon) ActivateUISprite();
            else DisableUISprite();
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
        playerWeaponInteraction.onWeaponDrop -= DisableUISprite;
        playerWeaponInteraction.onWeaponShot -= SetUIAmmoText;

        playerInventory.OnAmmoCountUpdate -= SetUIAmmoText;
    }

    void ActivateUISprite(Weapon weapon = null)
    {
        ammoUI.enabled = true;
        ammoSprite.sprite = weapon.SOWeapon.ammoSprite;
        SetUIAmmoText();
    }

    void DisableUISprite() 
    {
        ammoUI.enabled = false;
        ammoSprite.enabled = false;
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
}
