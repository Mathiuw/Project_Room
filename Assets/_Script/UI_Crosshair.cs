using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] RectTransform crosshairTransform;
    [SerializeField] RectTransform dot;
    [SerializeField] RectTransform reload;

    void Awake()
    {
        reload.gameObject.SetActive(false);
        SetDotCrosshair();
    }

    void Start() 
    {
        PlayerWeaponInteraction playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction)
        {
            playerWeaponInteraction.onWeaponPickup += SetCrossHair;
            playerWeaponInteraction.onAimStart += DisableCrosshair;
            playerWeaponInteraction.onAimEnd += EnableCrosshair;
            playerWeaponInteraction.onWeaponDrop += SetDotCrosshair;
            playerWeaponInteraction.onReloadStart += OnReloadFunc;
        }
    }

    void OnReloadFunc(float duration)
    {
        StartCoroutine(ReloadLerp(duration));
    }

    void EnableCrosshair() 
    {
        crosshairTransform.gameObject.SetActive(true);
    }

    void DisableCrosshair() 
    {
        crosshairTransform.gameObject.SetActive(false);
    }

    //Set the dafault crosshair
    void SetDotCrosshair() 
    {
        DestroyCrosshairSprite();
        SpawnCrosshairSprite(dot.gameObject);
    }

    void SpawnCrosshairSprite(GameObject crosshair) 
    {
        DestroyCrosshairSprite();

        if (crosshair) 
        {
            GameObject crosshairSprite = Instantiate(crosshair, crosshairTransform);
            crosshairSprite.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void DestroyCrosshairSprite()
    {
        for (int i = 0; i < crosshairTransform.childCount; i++)
        {
            Destroy(crosshairTransform.GetChild(i).gameObject);
        }
        Debug.Log("Destroyed crosshair sprite");
    }

    void SetCrossHair(Weapon weaponPicked)
    {
        SpawnCrosshairSprite(weaponPicked.crosshair);
    }

    IEnumerator ReloadLerp(float duration)
    {
        Image ring = reload.GetComponent<Image>();

        DisableCrosshair();
        reload.gameObject.SetActive(true);

        float timeElapsed = 0;

        ring.fillAmount = 0;

        while (timeElapsed < duration) 
        {
            ring.fillAmount = Mathf.Lerp(0, 1f, timeElapsed / duration);

            timeElapsed += Time.deltaTime;
            Debug.Log("Duration = " + duration + ", UI ring time elapsed = " + timeElapsed + ", UI ring fill amount = " + ring.fillAmount);

            yield return null;
        }

        ring.fillAmount = 1;

        reload.gameObject.SetActive(false);
        EnableCrosshair();

        yield break;
    }
}
