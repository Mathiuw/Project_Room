using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hit : MonoBehaviour
{
    public static UI_Hit Instance;

    AudioSource hitSound;
    [SerializeField] RectTransform[] hitSprite;

    void Awake() 
    {
        Instance= this;
        hitSound = GetComponent<AudioSource>();

        SetHitmarkerSprite(false);
    }

    void Start() 
    {
        if (Player.instance != null)
        {
            PlayerWeaponInteraction playerWeaponInteraction = Player.instance .GetComponent<PlayerWeaponInteraction>();

            playerWeaponInteraction.onPickupEnd += AddPlayerEvents;
            playerWeaponInteraction.onDrop += RemovePlayerEvents;
        }
    }

    public void OnHit(Health health) => StartCoroutine(Hitmarker(health));

    IEnumerator Hitmarker(Health health) 
    {
        if (health.Isdead()) yield break;
        hitSound.Play();
        SetHitmarkerSprite(true);
        yield return new WaitForSeconds(hitSound.clip.length);
        SetHitmarkerSprite(false);
        yield break;
    }

    void SetHitmarkerSprite(bool b) 
    {
        foreach (RectTransform r in hitSprite) r.gameObject.SetActive(b);
    }

    void AddPlayerEvents(Transform gun) 
    {
        PlayerWeaponInteraction playerWeaponInteraction= Player.instance.GetComponent<PlayerWeaponInteraction>();

        gun.GetComponent<weapon>().shootGun.onHit += OnHit;
    }

    void RemovePlayerEvents() 
    {
        PlayerWeaponInteraction playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();

        playerWeaponInteraction.currentWeapon.shootGun.onHit -= OnHit;
    }
}
