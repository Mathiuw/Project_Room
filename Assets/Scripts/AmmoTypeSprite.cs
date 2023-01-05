using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeSprite : MonoBehaviour
{
    [SerializeField] Image ammoType;
    [SerializeField] Sprite[] ammoTypeSprite;

    void Start() => Player.Instance.WeaponInteraction.onPickupCoroutineEnd += SetSprite;

    void SetSprite(Transform gun) => ammoType.sprite = ammoTypeSprite[whichSpriteReturn(gun)];

    int whichSpriteReturn(Transform gun)
    {
        string gunName = gun.GetComponent<Name>().text;

        if (gunName == "Double Barrel") return 0;
        else if (gunName == "MP5") return 1;
        else return 2;
    }
}
