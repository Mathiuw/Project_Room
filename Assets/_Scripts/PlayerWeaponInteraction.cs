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
    Vector3 weaponHolderPositon;

    public event Action<Transform, Ammo, bool> onShoot;
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

        weaponHolderPositon = gunHolder.transform.localPosition;
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

    void Shoot()
    {
        if (!isHoldingWeapon) return;
        if (isLerping) return;

        Ammo ammo = currentWeapon.GetComponent<Ammo>();

        onShoot?.Invoke(cameraTransform, ammo, currentWeapon.reloadGun.isReloading);
    }

    void Aim(bool b, Vector3 aimVector)
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        isAiming = b;

        StopAllCoroutines();
        StartCoroutine(LerpWeapon(aimTime, currentWeapon.transform, aimVector, Quaternion.identity));
    }

    void AimTrue()
    {
        if (!isHoldingWeapon) return;
        Vector3 aimVector = -weaponHolderPositon - currentWeapon.shootGun.GetAimVector();

        Aim(true, aimVector);
        onAimStart?.Invoke();
    }

    void AimFalse() 
    {
        Aim(false, Vector3.zero);
        onAimEnd?.Invoke();
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
        currentWeapon.SetHoldState(true, this);
        StartCoroutine(LerpWeapon(0.2f, weapon, Vector3.zero, Quaternion.identity, gunHolder));
        while (isLerping) yield return null;     

        isHoldingWeapon = true;
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

    public override IEnumerator ReloadWeapon()  
    {
        if (!isHoldingWeapon) yield break;
        if (isAiming) yield break;

        ReloadGun reloadGun = currentWeapon.reloadGun;
        Ammo ammo = currentWeapon.GetComponent<Ammo>();

        reloadGun.onReloadStart += UI_Inventory.instance.RefreshInventory;

        StartCoroutine(reloadGun.Reload(reloadGun.reloadTime, ammo, inventory));
        yield break;
    }

    public override void DropWeapon()
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        Transform weaponTransform = currentWeapon.transform;

        StopAllCoroutines();
        currentWeapon.SetHoldState(false, this);
        weaponTransform.SetParent(null);
        weaponTransform.localScale = Vector3.one;   

        isLerping = false;
        isAiming = false;
        isHoldingWeapon = false;
        currentWeapon = null;
        onDrop?.Invoke(weaponTransform);
        Debug.Log("Dropped weapon");
    }
}
