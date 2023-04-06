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
    float duration = 0;

    void Start() 
    {
        if (Player.instance != null) 
        {
            playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
            playerWeaponInteraction.onPickupEnd += SetCrossHair;
            playerWeaponInteraction.onAimStart += SetCrossHair;
            playerWeaponInteraction.onAimEnd += SetCrossHair;
            playerWeaponInteraction.onDrop += SetCrossHair;
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
            SpawnCrosshairSprite(dot.gameObject);
            return;
        }

        if (!playerWeaponInteraction.isAiming && !playerWeaponInteraction.isReloading)
        {
            SpawnCrosshairSprite(playerWeaponInteraction.currentWeapon.weaponSO.Crosshair);
            return;
        }

        if (playerWeaponInteraction.isAiming && !playerWeaponInteraction.isReloading)
        {
            DespawnSprite();
            return;
        }

        if (playerWeaponInteraction.isReloading)
        {
            DespawnSprite();
            if(!isLerping) StartCoroutine(ReloadLerp(duration));
        }
    }

    IEnumerator ReloadLerp(float duration)
    {
        Image ring = reload.GetComponent<Image>();

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
}
