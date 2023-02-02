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
    PlayerWeaponInteraction playerWeaponInteraction;
    Animator animator;
    ReloadGun reloadGun;

    void Start() 
    {
        ring = crosshair_ReloadRing.GetComponent<Image>();
        playerWeaponInteraction = Player.instance.GetComponentInChildren<PlayerWeaponInteraction>();
        animator = Player.instance.GetComponentInChildren<Animator>();
    } 

    void Update() => CrossHairCheck();

    void CrossHairCheck()
    {
        if (!playerWeaponInteraction.isHoldingWeapon)
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        reloadGun = Player.instance.GetComponentInChildren<ReloadGun>();
        reloadGun.ReloadStarted += StartRingFill;
        reloadGun.ReloadEnded += EndRingFill;
        duration = reloadGun.reloadTime;

        if (animator.GetBool("isAiming"))
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

    void StartRingFill()
    {
        ring.fillAmount = 0;
        timeStartedLerp = Time.time;
    }

    void EndRingFill()
    {
        ring.fillAmount = 1;
        percentageComplete = 0;
    }
}
