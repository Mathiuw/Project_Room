using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Ammo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoUI;
    PlayerWeaponInteraction playerWeaponInteraction;
    int ammo = 0;
    int maxAmmo = 0;

    void Start() 
    {
        if (Player.instance != null) playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
        else 
        {
            Debug.LogError("Cant Find Player");
            enabled = false;
        }
    }

    void Update()     
    {
        if (!playerWeaponInteraction.isHoldingWeapon) 
        {
            if(ammoUI.enabled) ammoUI.enabled = false;
            return;
        }
        SetPLayerAmmo();
        CheckAmmo(ammo, maxAmmo);
    }

    void SetPLayerAmmo() 
    {
        if (ammo != playerWeaponInteraction.currentWeapon.shootGun.ammo) ammo = playerWeaponInteraction.currentWeapon.shootGun.ammo;
        if (maxAmmo != playerWeaponInteraction.currentWeapon.shootGun.maximumAmmo) maxAmmo = playerWeaponInteraction.currentWeapon.shootGun.maximumAmmo;
    }

    void CheckAmmo(int ammo, int maxAmmo) 
    {
        if(!ammoUI.enabled) ammoUI.enabled = true;

        ammoUI.SetText(ammo + "/" + maxAmmo);
    }
}
