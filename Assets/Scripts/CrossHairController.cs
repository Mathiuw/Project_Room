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

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        CrossHairCheck();
    }

    private void CrossHairCheck()
    {
        if (!WeaponPickup.IsHoldingWeapon())
        {
            crosshair_Dot.SetActive(true);
            crosshair_Weapon.SetActive(false);
            crosshair_ReloadRing.SetActive(false);
            return;
        }

        ShootGun gunScript = transform.root.GetComponentInChildren<ShootGun>();

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
            StartCoroutine(FillReloadRing(gunScript));
            return;
        }

        crosshair_Dot.SetActive(false);
        crosshair_Weapon.SetActive(true);
        crosshair_ReloadRing.SetActive(false);
    }

    IEnumerator FillReloadRing(ShootGun gunScript)
    {
        Image ring = crosshair_ReloadRing.GetComponent<Image>();
        float timeElapsed = 0;
        float duration = gunScript.reloadTime * 25;

        ring.fillAmount = 0f;

        while (timeElapsed < duration )
        {
            ring.fillAmount = Mathf.Lerp(ring.fillAmount, 1f, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        ring.fillAmount = 1f;
    }
}
