using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    [SerializeField] GameObject crosshair_Dot;
    [SerializeField] GameObject crosshair_Weapon;
    [SerializeField] GameObject crosshair_ReloadRing;

    float timeStartedLerp = 0;
    float percentageComplete = 0;
    float duration;
    Image ring;
    ReloadGun reloadGun;

    void Start() => ring = crosshair_ReloadRing.GetComponent<Image>();

    void Update() => CrossHairCheck();

    void CrossHairCheck()
    {
        if (!Player.Instance.WeaponInteraction.isHoldingWeapon)
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        reloadGun = Player.Instance.GetComponentInChildren<ReloadGun>();
        reloadGun.ReloadStarted += StartRingFill;
        reloadGun.ReloadEnded += EndRingFill;
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
