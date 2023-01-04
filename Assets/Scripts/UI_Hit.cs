using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hit : MonoBehaviour
{
    float time = 0.15f;

    AudioSource hitSound;
    [SerializeField] RectTransform[] hitSprite;

    void Awake() 
    {
        hitSound = GetComponent<AudioSource>();

        SetHitSprite(false);
    }

    void Start() 
    {
        Player.Instance.WeaponPickup.onPickupCoroutineEnd += AddEventToGun;
    }

    void AddEventToGun(Transform g) => g.transform.GetComponent<ShootGun>().onHit += OnHit;

    void OnHit() => StartCoroutine(OnHitCoroutine());

    IEnumerator OnHitCoroutine() 
    {
        hitSound.Play();
        SetHitSprite(true);
        yield return new WaitForSeconds(hitSound.clip.length);
        SetHitSprite(false);
        yield break;
    }

    void SetHitSprite(bool b) 
    {
        foreach (RectTransform r in hitSprite) r.gameObject.SetActive(b);
    }
}
