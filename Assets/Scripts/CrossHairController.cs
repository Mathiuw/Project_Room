using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject crosshair_Dot;
    [SerializeField] private GameObject crosshair_Weapon;
    [SerializeField] private GameObject crosshair_ReloadRing;

    float timeStartedLerp = 0;
    float percentageComplete = 0;
    float duration;
    Image ring;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        ring = crosshair_ReloadRing.GetComponent<Image>();
    }
    private void Update()
    {
        CrossHairCheck();
    }

    private void CrossHairCheck()
    {
        if (!WeaponPickup.IsHoldingWeapon)
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        ShootGun gunScript = transform.root.GetComponentInChildren<ShootGun>();
        gunScript.OnReloadStart += StartRingFill;
        gunScript.OnReloadEnd += EndRingFill;
        duration = gunScript.reloadTime;

        if (animator.GetBool("isAiming"))
        {
            crosshair_Dot.SetActive(false);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        if (gunScript.reloading)
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

        var result = Mathf.Lerp(0, 1f, percentageComplete);
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
