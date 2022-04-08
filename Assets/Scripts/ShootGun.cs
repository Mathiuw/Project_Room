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
    public float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private int bulletMaxDistace = 100;
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
                Reload();
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

    private void Reload()
    {
        ammo = maximumAmmo;
    }

    public void CheckIfCanDo(bool check)
    {
        if (check) canDo = false;
        else canDo = true;
    }
}
