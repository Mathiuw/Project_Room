using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;

public class ShootGun : MonoBehaviour, ICanDo
{
    private bool canDo = true;

    private Transform playerCamera;
    [SerializeField] private LayerMask shootLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private ParticleSystem muzzleFlash;

    [HideInInspector] public bool beingHold;
    private Animator playerAnimator;
    private Rigidbody rb;
    private GameObject playerRef;
    private AudioSource gunSound;

    [Header("Weapon config")]
    public bool reloading = false;
    [SerializeField] private Items reloadMag;
    [SerializeField] float reloadTime = 4;
    [SerializeField] private int damage;
    [SerializeField] private int bulletMaxDistace = 100;
    public float fireRate;
    public int ammo;
    public int maximumAmmo;
    private float nextTimeToFire = 0;

    private void Awake()
    {
        playerRef = GameObject.Find("Player");
        playerCamera = GameObject.Find("Main Camera").transform;
        playerAnimator = playerRef.GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody>();
        gunSound = GetComponent<AudioSource>();
        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    private void Update()
    {
        if (beingHold)
        {
            if (Health.playerDead) return;
            if (!canDo) return;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                AnimatorStateInfo animatorState = playerAnimator.GetCurrentAnimatorStateInfo(0);

                if (reloading) return;
                if (animatorState.IsName("Start Reloading") || animatorState.IsName("End Reloading")) return;

                if (ammo == 0)
                {
                    playerAnimator.SetBool("isShooting", false);
                    return;
                }
                if (Time.time > +nextTimeToFire && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shooting") && !playerAnimator.IsInTransition(0))
                {
                    playerAnimator.SetBool("isShooting", true);
                    nextTimeToFire = Time.time + (1f / fireRate);
                    Shoot();
                }
                else playerAnimator.SetBool("isShooting", false);
            }
            else playerAnimator.SetBool("isShooting", false);

            if (Input.GetKeyDown(KeyCode.R))
            {
                Inventory inventoryScript = playerRef.GetComponent<Inventory>();
                UI_Inventory uiIventoryScript = playerRef.transform.parent.GetComponentInChildren<UI_Inventory>();

                if (inventoryScript.HasItemOnInventory(reloadMag) && !playerAnimator.GetBool("isAiming") && !playerAnimator.GetBool("isShooting") && !reloading && ammo != maximumAmmo)
                {
                    inventoryScript.CheckAndRemoveItem(reloadMag);
                    uiIventoryScript.RefreshInventory();
                    StartCoroutine(Reload());
                }
            }
        }
        else if (transform.parent != null && transform.parent.CompareTag("Enemy"))
        {
            if (!GetComponentInParent<EnemyAi>().IsDead())
            {
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rb.isKinematic = true;
            }
            else
            {
                transform.parent = null;
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
    }

    private void Shoot()
    {
        RaycastHit hit;  
        
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, bulletMaxDistace, shootLayer))
        {
            if (hit.transform.GetComponent<EnemyAi>())
            {
                hit.transform.GetComponent<EnemyAi>().TakeDamage(damage);
                Debug.Log(hit.transform.name + "life = " + hit.transform.GetComponent<EnemyAi>().health);
            }
            playerCamera.GetComponentInParent<CamFollowAndShake>().shakeDuration += 0.1f;
            muzzleFlash.Play(true);
            gunSound.Play();
            ammo--;
        }
    }

    public void EnemyShoot(Transform enemyTransfom)
    {
        RaycastHit hit;

        if (Physics.Raycast(enemyTransfom.position, enemyTransfom.forward, out hit, bulletMaxDistace, playerLayer))
        {
            muzzleFlash.Play(true);
            gunSound.Play();

            if (hit.transform.name == "Player")
            {
                Health.RemoveHealth(damage / 3);
                Debug.Log("Player hit");

                if (Health.playerDead)
                {
                    Rigidbody playerRB = playerRef.GetComponent<Rigidbody>();
                    playerRB.AddForce(enemyTransfom.forward * 2, ForceMode.VelocityChange);
                }
            }
        }
    }

    IEnumerator Reload()
    {
        string gunName = GetComponent<Name>().text;

        Debug.Log("Start Reload");
        playerAnimator.SetBool("isShooting",false);
        playerAnimator.SetBool("isAiming", false);
        reloading = true;
        playerAnimator.Play(gunName + " Start Reloading", 0);

        yield return new WaitForSeconds(reloadTime);

        playerAnimator.SetTrigger("ReloadEnd");
        ammo = maximumAmmo;
        reloading = false;
        Debug.Log("Reload Finished");
        yield break;
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}
