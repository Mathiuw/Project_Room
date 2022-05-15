using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeSprite : MonoBehaviour
{
    [SerializeField] Image ammoType;
    [SerializeField]Sprite[] ammoTypeSprite;

    void Update()
    {
        FindObjectOfType<WeaponPickup>().OnWeaponPickup += SetSprite;
    }

    void SetSprite()
    {
        ammoType.sprite = ammoTypeSprite[whichSpriteReturn()];
    }

    int whichSpriteReturn()
    {
        string gunName = GameObject.Find("Gun_holder").transform.GetChild(0).transform.name;

        if (gunName == "DoubleBarrel") return 0;
        else if (gunName == "Mp5") return 1;
        else return 2;
    }
}
