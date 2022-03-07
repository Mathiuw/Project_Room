using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour, ICanDo
{
    [SerializeField] private bool canDo = true;

    [SerializeField] private GameObject playerCamera;
    [SerializeField] private LayerMask shootLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private ParticleSystem muzzleFlash;
    private Animator playerAnimator;
    private Rigidbody rb;
    private GameObject playerRef;
    [HideInInspector] public bool beingHold;

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
        playerCamera = GameObject.Find("Main Camera");
        playerAnimator = playerRef.GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    private void Update()
    {
        if (beingHold)
        {
            if (!Health.playerDead)
            {
                if (Input.GetKey(KeyCode.Mouse0) && canDo)
                {
                    if (ammo > 0)
                    {
                        playerAnimator.SetBool("isShooting", true);

                        if (Time.time > +nextTimeToFire)
                        {
                            nextTimeToFire = Time.time + (1f / fireRate);
                            Shoot();
                        }
                    }
                    else playerAnimator.SetBool("isShooting", false);
                }
                else playerAnimator.ResetTrigger("isShooting");

                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                }
            }
        }
        else if (transform.parent != null && transform.parent.CompareTag("Enemy"))
        {
            if (!GetComponentInParent<EnemyAi>().IsDead())
            {
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rb.isKinematic = true;
            }
            else
            {
                transform.parent = null;
                rb.isKinematic = false;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }
    }

    private void Shoot()
    {
        RaycastHit hit;  
        
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, bulletMaxDistace, shootLayer))
        {
            if (hit.transform.GetComponent<EnemyAi>())
            {
                hit.transform.GetComponent<EnemyAi>().TakeDamage(damage);
                Debug.Log("Enemy hit");
                Debug.Log(hit.transform.name + "life = " + hit.transform.GetComponent<EnemyAi>().health);
            }
            FindObjectOfType<AudioManager>().Play("Smg Shot");
            playerCamera.GetComponentInParent<CamFollowAndShake>().shakeDuration += 0.1f;
            muzzleFlash.Play(true);
            ammo--;
        }
    }

    public void EnemyShoot(Transform enemyTransfom)
    {
        RaycastHit hit;

        muzzleFlash.Play(true);
        FindObjectOfType<AudioManager>().Play("Smg Shot");

        if (Physics.Raycast(enemyTransfom.position, enemyTransfom.forward, out hit, bulletMaxDistace, playerLayer))
        {
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
        if (check)
        {
            canDo = false;
        }
        else canDo = true;
    }
}
