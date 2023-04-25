using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] RectTransform crosshairTransform;
    [SerializeField] RectTransform dot;
    [SerializeField] RectTransform reload;
    PlayerWeaponInteraction playerWeaponInteraction;
    bool isLerping = false;

    void Start() 
    {
        if (Player.instance != null) 
        {
            playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
            playerWeaponInteraction.onPickupEnd += SetCrossHair;
            playerWeaponInteraction.onAimStart += SetCrossHair;
            playerWeaponInteraction.onAimEnd += SetCrossHair;
            playerWeaponInteraction.onDrop += SetCrossHair;
            playerWeaponInteraction.onReloadStart += SetCrossHair;
            playerWeaponInteraction.onReloadEnd += SetCrossHair;
            SetCrossHair();
            reload.gameObject.SetActive(false);
        } 
        else 
        {
            Debug.LogError("Cant Find Player");
            Destroy(this);
        }
    }

    void DespawnSprite() 
    {
        for (int i = 0; i < crosshairTransform.childCount; i++)
        {
            Destroy(crosshairTransform.GetChild(i).gameObject);
        }
    }

    void SpawnCrosshairSprite(GameObject crosshair) 
    {
        DespawnSprite();

        GameObject crosshairSprite = Instantiate(crosshair, crosshairTransform);
        crosshairSprite.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void SetCrossHair()
    {
        if (!playerWeaponInteraction.isHoldingWeapon) 
        {
            Debug.Log("UI dot crosshair");
            SpawnCrosshairSprite(dot.gameObject);
            return;
        }

        if (!playerWeaponInteraction.isAiming && !playerWeaponInteraction.isReloading)
        {
            Debug.Log("UI weapon crosshair");
            SpawnCrosshairSprite(playerWeaponInteraction.currentWeapon.weaponSO.Crosshair);
            return;
        }

        if (playerWeaponInteraction.isAiming && !playerWeaponInteraction.isReloading)
        {
            Debug.Log("UI aim crosshair");
            DespawnSprite();
            return;
        }

        if (playerWeaponInteraction.isReloading)
        {
            Debug.Log("UI reload crosshair");
            DespawnSprite();
            if (!isLerping) StartCoroutine(ReloadLerp());
        }
    }

    IEnumerator ReloadLerp()
    {
        Image ring = reload.GetComponent<Image>();

        reload.gameObject.SetActive(true);

        float timeElapsed = 0;
        float duration = playerWeaponInteraction.currentWeapon.weaponSO.reloadTime;

        isLerping = true;
        ring.fillAmount = 0;

        while (timeElapsed < duration) 
        {
            ring.fillAmount = Mathf.Lerp(0, 1f, timeElapsed / duration);

            timeElapsed += Time.deltaTime;
            Debug.Log("Duration = " + duration + ", UI ring time elapsed = " + timeElapsed + ", UI ring fill amount = " + ring.fillAmount);

            yield return null;
        }

        ring.fillAmount = 1;
        isLerping = false;

        reload.gameObject.SetActive(false);

        yield break;
    }
}
