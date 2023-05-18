using System.Collections;
using UnityEngine;

public class UI_Hit : MonoBehaviour
{
    public static UI_Hit Instance;

    AudioSource hitSound;
    PlayerWeaponInteraction playerWeaponInteraction;
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
            playerWeaponInteraction = Player.instance .GetComponent<PlayerWeaponInteraction>();
            playerWeaponInteraction.onPickupEnd += AddPlayerEvents;
            playerWeaponInteraction.onDrop += RemovePlayerEvents;
        }
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

    void AddPlayerEvents() 
    {
        playerWeaponInteraction.currentWeapon.GetComponent<WeaponShoot>().onHit += OnHit;
    }

    void RemovePlayerEvents() 
    {
        playerWeaponInteraction.currentWeapon.GetComponent<WeaponShoot>().onHit -= OnHit;
    }
}
