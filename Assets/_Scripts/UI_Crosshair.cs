using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] RectTransform dot;
    [SerializeField] RectTransform weapon;
    [SerializeField] RectTransform reload;
    PlayerWeaponInteraction playerWeaponInteraction;
    bool isHoldingWeapon;
    bool isAiming;
    bool isReloading;
    bool isLerping = false;
    float duration = 0;


    void Start() 
    {
        if (Player.instance != null) 
        {
            playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
        } 
        else 
        {
            Debug.LogError("Cant Find Player");
            enabled = false;
        }
    }

    void Update() 
    {
        if (playerWeaponInteraction.isHoldingWeapon) 
        {
            if (isLerping && !isReloading) ResetLerp();

            isHoldingWeapon = playerWeaponInteraction.isHoldingWeapon;
            isAiming = playerWeaponInteraction.isAiming;
            isReloading = playerWeaponInteraction.currentWeapon.reloadGun.isReloading;
            duration = playerWeaponInteraction.currentWeapon.reloadGun.reloadTime;

            SetCrossHair(isHoldingWeapon, isReloading, isAiming, duration);
        } 
        else SetCrossHair();
    }

    void SetSprites(RectTransform crosshair, bool b = false) 
    {
        if (crosshair.gameObject.activeSelf == b) return; 
        crosshair.gameObject.SetActive(b);
    }

    public void SetCrossHair(bool isHolding = false, bool isReloading = false, bool isAiming = false, float duration = 0)
    {
        if (!isHolding) 
        {
            SetSprites(dot, true);
            SetSprites(weapon);
            SetSprites(reload);
            return;
        }

        if (!isAiming && !isReloading)
        {
            SetSprites(dot);
            SetSprites(weapon, true);
            SetSprites(reload);
            return;
        }

        if (isAiming && !isReloading)
        {
            SetSprites(dot);
            SetSprites(weapon);
            SetSprites(reload);
            return;
        }

        if (isReloading)
        {
            SetSprites(dot);
            SetSprites(weapon);
            SetSprites(reload, true);
            if(!isLerping) StartCoroutine(ReloadLerp(duration, reload));
        }
    }

    IEnumerator ReloadLerp(float duration, RectTransform crosshair)
    {
        Image ring = crosshair.GetComponent<Image>();

        float timeElapsed = 0;

        isLerping = true;

        ring.fillAmount = 0;
        while (timeElapsed < duration) 
        {
            ring.fillAmount = Mathf.Lerp(0, 1f, timeElapsed);

            timeElapsed += Time.deltaTime/ duration;

            yield return null;
        }
        ring.fillAmount = 1;

        isLerping = false;

        yield break;
    }

    void ResetLerp() 
    {
        isLerping = false;
        StopAllCoroutines();   
    }
}
