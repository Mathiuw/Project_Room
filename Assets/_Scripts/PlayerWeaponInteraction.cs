using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerWeaponInteraction : WeaponInteraction
{
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxPickupDistance;
    [SerializeField] float aimTime;
    
    public bool isAiming { get; private set; } = false;
    public bool isReloading { get; private set; } = false;

    bool isLerping = false;

    Transform cameraTransform;
    Inventory inventory;
    CameraShake cameraShake;

    //Set hold position for whem mot aiming
    Vector3 defautHoldPosition;

    public event Action onWeaponShot;
    public event Action onAimStart;
    public event Action onAimEnd;
    public event Action<Weapon> onPickupWeapon;
    public event Action<float> onReloadStart;
    public event Action onReloadEnd;
    public event Action onDrop;

    void Awake() 
    {
        inventory = GetComponent<Inventory>();
    } 

    void Start() 
    {
        cameraTransform = Camera.main.transform;

        PlayerCamera playerCamera = FindAnyObjectByType<PlayerCamera>();

        if (playerCamera) 
        {
            weaponHolder = playerCamera.GunHolder;
            cameraShake = playerCamera.GetComponent<CameraShake>();
        } 

        // set holder position for aimming
        defautHoldPosition = weaponHolder.transform.localPosition;
    } 

    void Update()
    {
        Shoot();

        if (Input.GetKeyDown(KeyCode.Mouse1)) AimTrue();
        else if(Input.GetKeyUp(KeyCode.Mouse1)) AimFalse();

        if (Input.GetKeyDown(KeyCode.E)) TryToPickupWeapon();

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

        if (Input.GetKeyDown(KeyCode.G)) DropWeapon();
    }

    IEnumerator LerpWeapon(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation, Transform parent = null) 
    {
        isLerping = true;

        if (parent != null)
        {
            weapon.SetParent(parent);
            weapon.localScale = Vector3.one;
        }

        float elapsedTime = 0f;
        float percentageComplete = 0f;

        Vector3 startPosition = weapon.localPosition;
        Quaternion startRotation = weapon.localRotation;

        while (elapsedTime < time)
        {
            weapon.localPosition = Vector3.Lerp(startPosition, desiredPosition, percentageComplete);
            weapon.localRotation = Quaternion.Lerp(startRotation, desiredRotation, percentageComplete);

            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / time;
            yield return null;
        }

        weapon.localPosition = desiredPosition;
        weapon.localRotation = desiredRotation;
        isLerping = false;
    }

    public void TryToPickupWeapon()
    {
        if (isHoldingWeapon) return;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxPickupDistance, WeaponMask))
        {
            Weapon weaponSelected = hit.transform.GetComponentInParent<Weapon>();

            if (weaponSelected) 
            {
                StopAllCoroutines();
                StartCoroutine(PickUpWeapon(weaponSelected));
                Debug.Log("Picked up " + weaponSelected.name);
                return;
            } 
            
        }
        Debug.Log("Find nothing");
    }

    protected override IEnumerator PickUpWeapon(Weapon pickedWeapon)
    {
        if (isHoldingWeapon) yield break;
        if (isLerping) yield break;
        if (pickedWeapon.holder != null)
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        //Set current weapon
        weapon = pickedWeapon;

        //Set weapon hold state
        weapon.SetHoldState(true, transform);
        
        //Lerp weapon to player
        StartCoroutine(LerpWeapon(0.2f, pickedWeapon.transform, Vector3.zero, Quaternion.identity, weaponHolder));
        while (isLerping) yield return null;

        isHoldingWeapon = true;
        //SetWeaponEvents(true);
        onPickupWeapon?.Invoke(base.weapon);

        Debug.Log("Picked up weapon");
    }

    bool InputShoot() 
    {
        switch (weapon.shootType) 
        {
            case Weapon.ShootType.Single: 
                return Input.GetKeyDown(KeyCode.Mouse0);
            case Weapon.ShootType.Automatic:
                return Input.GetKey(KeyCode.Mouse0);
            default: return false;
        }
    }

    void Shoot()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;
        if (isLerping) return;

        if (InputShoot()) 
        {
            weapon.Shoot(cameraTransform);
            AddWeaponCameraShake();

            onWeaponShot?.Invoke();
        } ;
    }

    void Aim(bool b, Vector3 aimVector)
    {
        isAiming = b;

        StopAllCoroutines();
        StartCoroutine(LerpWeapon(aimTime, weapon.transform, aimVector, Quaternion.identity));
    }

    void AimTrue()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        Vector3 aimVector = -defautHoldPosition - weapon.GetAimLocation();

        Aim(true, aimVector);
        onAimStart?.Invoke();
    }

    void AimFalse() 
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        Aim(false, Vector3.zero);
        onAimEnd?.Invoke();
    }

    public override IEnumerator ReloadWeapon()  
    {
        if (!isHoldingWeapon) yield break;
        if (isReloading) yield break;
        if (isAiming) yield break;

        if(weapon.GetAmmo() == weapon.GetMaxAmmo()) yield break;

        if (!inventory.HaveItem(weapon.GetReloadItem())) yield break;
        inventory.RemoveItem(weapon.GetReloadItem());

        isReloading = true;
        onReloadStart?.Invoke(weapon.GetMaxAmmo());

        yield return new WaitForSeconds(weapon.GetReloadTime());

        weapon.AddAmmo(weapon.GetMaxAmmo());
        isReloading = false;
        onReloadEnd?.Invoke();

        yield break;
    }

    public override void DropWeapon()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        StopAllCoroutines();

        Transform currentWeaponTransform = weapon.transform;

        weapon.SetHoldState(false, null);
        //SetWeaponEvents(false);
        currentWeaponTransform.SetParent(null);
        currentWeaponTransform.localScale = Vector3.one;

        isLerping = false;
        isAiming = false;
        isReloading = false;
        isHoldingWeapon = false;

        onDrop?.Invoke();

        weapon = null;
        Debug.Log("Dropped weapon");
    }

    void AddWeaponCameraShake() 
    {
        cameraShake.AddCameraShake(weapon.GetIntensity(), weapon.GetSpeed());
    }

    void SetWeaponEvents(bool b) 
    {
        if (!isHoldingWeapon) return;
        
        WeaponAnimationManager weaponAnimationManager = weapon.GetComponent<WeaponAnimationManager>();
        Animator animator = weapon.GetComponent<Animator>();

        animator.enabled = b;

        if (b) 
        {         
            weapon.onShoot += weaponAnimationManager.ShootWeaponAnimation;
        }
        else
        {
            weapon.onShoot -= weaponAnimationManager.ShootWeaponAnimation;
        }
    }
}
