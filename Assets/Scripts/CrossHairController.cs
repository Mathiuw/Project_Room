using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    [SerializeField] private GameObject crosshair_Dot;
    [SerializeField] private GameObject crosshair_Weapon;
    [SerializeField] private GameObject crosshair_ReloadRing;

    float timeStartedLerp = 0;
    float percentageComplete = 0;
    float duration;
    Image ring;

    private void Awake() => ring = crosshair_ReloadRing.GetComponent<Image>();

    private void Update() => CrossHairCheck();

    private void CrossHairCheck()
    {
        if (!Player.Instance.WeaponPickup.IsholdingWeapon())
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        ReloadGun reloadGun = Player.Instance.GetComponentInChildren<ReloadGun>();

        reloadGun.OnReloadStart += StartRingFill;
        reloadGun.OnReloadEnd += EndRingFill;
        duration = reloadGun.reloadTime;

        if (Player.Instance.Animator.GetBool("isAiming"))
        {
            crosshair_Dot.SetActive(false);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        if (reloadGun.reloading)
        {
            crosshair_Dot.SetActive(false);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(true);
            ring.fillAmount = ReloadRingLerp();
            return;
        }

        crosshair_Dot.SetActive(false);
        crosshair_Weapon.SetActive(true);
        crosshair_ReloadRing.SetActive(false);
    }

    float ReloadRingLerp()
    {
        float timeSinceStarted = Time.time - timeStartedLerp;
        percentageComplete = timeSinceStarted / duration;

        float result = Mathf.Lerp(0, 1f, percentageComplete);
        return result;
    }

    private void StartRingFill()
    {
        ring.fillAmount = 0;
        timeStartedLerp = Time.time;
    }

    private void EndRingFill()
    {
        ring.fillAmount = 1;
        percentageComplete = 0;
    }
}
