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
    PlayerDrop playerDrop;
    CameraShake cameraShake;
    //Set hold position for whem mot aiming
    Vector3 defautHoldPosition;

    public event Action<Transform> onShoot;
    public event Action onAimStart;
    public event Action onAimEnd;
    public event Action onPickupEnd;
    public event Action onReloadStart;
    public event Action onReloadEnd;
    public event Action onDrop;

    void Awake() 
    {
        inventory = GetComponent<Inventory>();
        playerDrop = GetComponent<PlayerDrop>();
    } 

    void Start() 
    {
        cameraTransform = Camera.main.transform;

        if (PlayerCamera.instance != null) 
        {
            weaponHolder = PlayerCamera.instance.GunHolder;
            cameraShake = PlayerCamera.instance.GetComponent<CameraShake>();
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

    protected override IEnumerator PickUpWeapon(Transform weapon)
    {
        if (isHoldingWeapon) yield break;
        if (isLerping) yield break;

        //Set current weapon
        currentWeapon = weapon.GetComponent<Weapon>();

        if (currentWeapon.isBeingHold)
        {
            currentWeapon = null;
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        //Add components
        currentWeapon.AddComponent<WeaponShoot>();
        currentWeapon.AddComponent<WeaponAnimationManager>();
        currentWeapon.AddComponent<WeaponParticlesManager>();

        //Set weapon hold state
        currentWeapon.SetHoldState(true, transform);
        
        //Lerp weapon to player
        StartCoroutine(LerpWeapon(0.2f, weapon, Vector3.zero, Quaternion.identity, weaponHolder));
        while (isLerping) yield return null;

        isHoldingWeapon = true;
        SetWeaponEvents(true);
        onPickupEnd?.Invoke();

        Debug.Log("Picked up weapon");
    }

    public void TryToPickupWeapon()
    {
        if (isHoldingWeapon) return;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxPickupDistance, WeaponMask)) 
        {
            if (hit.transform.GetComponent<Weapon>()) StartCoroutine(PickUpWeapon(hit.transform));
        }
    }

    bool InputShoot() 
    {
        switch (currentWeapon.weaponSO.shootType) 
        {
            case SOWeapon.ShootType.Single: 
                return Input.GetKeyDown(KeyCode.Mouse0);
            case SOWeapon.ShootType.Automatic:
                return Input.GetKey(KeyCode.Mouse0);
            default: return false;
        }
    }

    void Shoot()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;
        if (isLerping) return;

        if (InputShoot()) onShoot?.Invoke(cameraTransform);
    }

    void Aim(bool b, Vector3 aimVector)
    {
        isAiming = b;

        StopAllCoroutines();
        StartCoroutine(LerpWeapon(aimTime, currentWeapon.transform, aimVector, Quaternion.identity));
    }

    void AimTrue()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        WeaponLocations weaponLocations = currentWeapon.GetComponentInChildren<WeaponLocations>();

        Vector3 aimVector = -defautHoldPosition - weaponLocations.GetAimLocation();

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

        WeaponAmmo ammo = currentWeapon.GetComponent<WeaponAmmo>();

        if(ammo.ammo == ammo.maxAmmo) yield break;

        if (!inventory.HaveItem(currentWeapon.weaponSO.reloadItem)) yield break;
        inventory.RemoveItem(currentWeapon.weaponSO.reloadItem);

        if (UI_Inventory.instance != null) UI_Inventory.instance.RefreshInventory();

        isReloading = true;
        onReloadStart?.Invoke();

        yield return new WaitForSeconds(currentWeapon.weaponSO.reloadTime);

        ammo.AddAmmo(ammo.maxAmmo);
        isReloading = false;
        onReloadEnd?.Invoke();

        yield break;
    }

    public override void DropWeapon()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        Transform currentWeaponTransform = currentWeapon.transform;

        StopAllCoroutines();
        currentWeapon.SetHoldState(false, null);
        SetWeaponEvents(false);
        currentWeaponTransform.SetParent(null);
        currentWeaponTransform.localScale = Vector3.one;

        Destroy(currentWeapon.GetComponent<WeaponShoot>());
        Destroy(currentWeapon.GetComponent<WeaponAnimationManager>());
        Destroy(currentWeapon.GetComponent<WeaponParticlesManager>());

        isLerping = false;
        isAiming = false;
        isReloading = false;
        isHoldingWeapon = false;

        playerDrop.Drop(currentWeaponTransform);
        onDrop?.Invoke();

        currentWeapon = null;
        Debug.Log("Dropped weapon");
    }

    void WeaponCameraShake() 
    {
        cameraShake.AddCameraShake(currentWeapon.weaponSO.intensity, currentWeapon.weaponSO.speed);
    }

    void SetWeaponEvents(bool b) 
    {
        if (!isHoldingWeapon) return;

        WeaponShoot weaponShoot = currentWeapon.GetComponent<WeaponShoot>();
        WeaponAnimationManager weaponAnimationManager = currentWeapon.GetComponent<WeaponAnimationManager>();
        Animator animator = currentWeapon.GetComponentInChildren<Animator>();

        animator.enabled = b;

        if (b) 
        {
            onAimStart += currentWeapon.SetAimTrue;
            onAimEnd += currentWeapon.SetAimFalse;
            onDrop += currentWeapon.DropAim;
            onShoot += weaponShoot.Shoot;
            
            weaponShoot.onShoot += WeaponCameraShake;
            weaponShoot.onShoot += weaponAnimationManager.ShootWeapon;
        }
        else
        {
            onAimStart -= currentWeapon.SetAimTrue;
            onAimEnd -= currentWeapon.SetAimFalse;
            onDrop -= currentWeapon.DropAim;
            onShoot -= weaponShoot.Shoot;

            weaponShoot.onShoot -= WeaponCameraShake;
            weaponShoot.onShoot -= weaponAnimationManager.ShootWeapon;
        }
    }
}
