using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeSprite : MonoBehaviour
{
    [SerializeField] Image ammoType;
    [SerializeField] Sprite[] ammoTypeSprite;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Start() 
    {
        if (Player.instance != null) playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
    }

    void Update() 
    {
        if (!playerWeaponInteraction.isHoldingWeapon) 
        {
            if (ammoType.enabled) ammoType.enabled = false;
            return;
        } 
        SetSprite();
    }

    void SetSprite() 
    {
        if (!ammoType.enabled) ammoType.enabled= true;

        if(ammoType.sprite != ammoTypeSprite[whichSpriteReturn()]) ammoType.sprite = ammoTypeSprite[whichSpriteReturn()];
    } 

    int whichSpriteReturn()
    {
        string gunName = playerWeaponInteraction.currentWeapon.GetComponent<Name>().text;

        if (gunName == "Double Barrel") return 0;
        else if (gunName == "MP5") return 1;
        else return 2;
    }
}
