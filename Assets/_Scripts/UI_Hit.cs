using System.Collections;
using UnityEngine;

public class UI_Hit : MonoBehaviour
{
    AudioSource hitSound;
    PlayerWeaponInteraction playerWeaponInteraction;
    [SerializeField] RectTransform[] hitSprite;

    void Awake() 
    {
        hitSound = GetComponent<AudioSource>();

        SetHitmarkerSprite(false);
    }

    void Start() 
    {
        playerWeaponInteraction = FindObjectOfType<Player>().GetComponent<PlayerWeaponInteraction>();
        playerWeaponInteraction.onPickupEnd += AddPlayerEvents;
        playerWeaponInteraction.onDrop += RemovePlayerEvents;
    }

    public void OnHit(Health health) => StartCoroutine(Hitmarker(health));

    IEnumerator Hitmarker(Health health) 
    {
        if (health.isDead) yield break;
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

    void AddPlayerEvents(Weapon weaponPicked) 
    {
        playerWeaponInteraction.Weapon.GetComponent<Weapon>().onHit += OnHit;
    }

    void RemovePlayerEvents() 
    {
        playerWeaponInteraction.Weapon.GetComponent<Weapon>().onHit -= OnHit;
    }
}
