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

    public void OnPickupWeapon(Transform gun) => gun.GetComponent<weapon>().shootGun.onHit += OnHit;

    public void OnHit(Health health) => StartCoroutine(HitmarkerCoroutine(health));

    IEnumerator HitmarkerCoroutine(Health health) 
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
}
