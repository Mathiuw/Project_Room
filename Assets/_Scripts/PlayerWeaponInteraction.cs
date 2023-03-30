using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerWeaponInteraction : WeaponInteraction
{
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxPickupDistance;
    [SerializeField] float aimTime;
    
    public bool isAiming { get; private set; } = false;
    bool isLerping = false;

    Transform cameraTransform;
    Inventory inventory;
    Vector3 defautHolderPosition;

    public event Action<Transform> onShoot;
    public event Action onAimStart;
    public event Action onAimEnd;
    public event Action<Transform> onPickupStart;
    public event Action<Transform> onPickupEnd;
    public event Action<Transform> onDrop;

    void Awake() => inventory = GetComponent<Inventory>();

    void Start() 
    {
        cameraTransform = Camera.main.transform;

        if (PlayerCamera.instance != null) gunHolder = PlayerCamera.instance.GunHolder;

        defautHolderPosition = gunHolder.transform.localPosition;
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

        currentWeapon = weapon.GetComponent<Weapon>();

        if (currentWeapon.isBeingHold)
        {
            currentWeapon = null;
            Debug.LogError("Gun already Picked up");
            yield break;
        }
        onPickupStart?.Invoke(weapon);
        currentWeapon.SetHoldState(true, transform);
        
        StartCoroutine(LerpWeapon(0.2f, weapon, Vector3.zero, Quaternion.identity, gunHolder));
        while (isLerping) yield return null;

        isHoldingWeapon = true;
        SetWeaponEvents(true);
        onPickupEnd?.Invoke(weapon);
        Debug.Log("Picked up weapon");
    }

    public void TryToPickupWeapon()
    {
        if (isHoldingWeapon) return;

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxPickupDistance, WeaponMask))
            StartCoroutine(PickUpWeapon(hit.transform));
    }

    void Shoot()
    {
        if (!isHoldingWeapon) return;
        if (isLerping) return;

        Ammo ammo = currentWeapon.GetComponent<Ammo>();

        onShoot?.Invoke(cameraTransform);
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
        if (currentWeapon.reloadGun.isReloading) return;

        WeaponLocations weaponLocations = currentWeapon.GetComponentInChildren<WeaponLocations>();

        Vector3 aimVector = -defautHolderPosition - weaponLocations.GetAimLocation();

        Aim(true, aimVector);
        onAimStart?.Invoke();
    }

    void AimFalse() 
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        Aim(false, Vector3.zero);
        onAimEnd?.Invoke();
    }

    public override IEnumerator ReloadWeapon()  
    {
        if (!isHoldingWeapon) yield break;
        if (isAiming) yield break;

        WeaponReload reloadGun = currentWeapon.reloadGun;
        Ammo ammo = currentWeapon.GetComponent<Ammo>();

        reloadGun.onReloadStart += UI_Inventory.instance.RefreshInventory;

        StartCoroutine(reloadGun.Reload(inventory));
        yield break;
    }

    public override void DropWeapon()
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        Transform weaponTransform = currentWeapon.transform;

        StopAllCoroutines();
        currentWeapon.SetHoldState(false, null);
        SetWeaponEvents(false);
        weaponTransform.SetParent(null);
        weaponTransform.localScale = Vector3.one;   

        isLerping = false;
        isAiming = false;
        isHoldingWeapon = false;
        currentWeapon = null;
        onDrop?.Invoke(weaponTransform);
        Debug.Log("Dropped weapon");
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

            onShoot += weaponShoot.InputShoot;
            weaponShoot.onShoot += weaponAnimationManager.ShootWeapon;
        }
        else
        {
            onAimStart -= currentWeapon.SetAimTrue;
            onAimEnd -= currentWeapon.SetAimFalse;
            onDrop -= currentWeapon.DropAim;

            onShoot -= weaponShoot.InputShoot;
            weaponShoot.onShoot -= weaponAnimationManager.ShootWeapon;
        }
    }
}
