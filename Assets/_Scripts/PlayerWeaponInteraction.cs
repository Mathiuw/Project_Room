using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerWeaponInteraction : WeaponInteraction
{
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxPickupDistance;
    [SerializeField] float dropForce;

    public bool isAiming { get; private set; } = false;
    bool isLerping = false;

    Transform cameraTransform;
    Inventory inventory;

    public event Action onAimStart;
    public event Action onAimEnd;
    public event Action<Transform> onPickupStart;
    public event Action<Transform> onPickupEnd;
    public event Action onDrop;

    void Awake() => inventory = GetComponent<Inventory>();

    void Start() 
    {
        cameraTransform = Camera.main.transform;

        if (PlayerCamera.instance != null) gunHolder = PlayerCamera.instance.GunHolder;
    } 

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) Shoot();

        if (Input.GetKeyDown(KeyCode.Mouse1)) AimTrue();
        else if(Input.GetKeyUp(KeyCode.Mouse1)) AimFalse();

        if (Input.GetKeyDown(KeyCode.E)) TryToPickupWeapon();

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

        if (Input.GetKeyDown(KeyCode.G)) DropGun();
    }

    void Shoot() 
    {
        if (!isHoldingWeapon) return;
        if (isLerping) return;

        currentWeapon.shootGun.Shoot(cameraTransform);
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

        yield break;
    }

    void AimTrue()
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        isAiming = true;

        StopAllCoroutines();
        StartCoroutine(LerpWeapon(0.2f, currentWeapon.transform, currentWeapon.shootGun.GetAimVector(), Quaternion.identity));
        onAimStart?.Invoke();
    }

    void AimFalse() 
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        isAiming = false;

        StopAllCoroutines();
        StartCoroutine(LerpWeapon(0.2f, currentWeapon.transform, Vector3.zero, Quaternion.identity));
        onAimEnd?.Invoke();
    }

    protected override IEnumerator PickUpWeapon(Transform weapon)
    {
        if (isHoldingWeapon) yield break;

        enabled = false;

        currentWeapon = weapon.GetComponent<weapon>();

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
        enabled = true;
        onPickupEnd?.Invoke(weapon);

        Debug.Log("Picked up weapon");
        yield break;
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
        reloadGun.onReloadStart += UI_Inventory.instance.RefreshInventory;

        StartCoroutine(reloadGun.Reload(reloadGun.reloadTime, inventory));
        yield break;
    }

    public override void DropGun()
    {
        if (!isHoldingWeapon) return;
        if (isLerping) return;
        if (currentWeapon.reloadGun.isReloading) return;

        StopAllCoroutines();
        currentWeapon.SetHoldState(false);
        currentWeapon.transform.SetParent(null);
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.transform.localPosition = transform.position + transform.forward * 1.5f;
        currentWeapon.transform.rotation = transform.rotation;
        currentWeapon.rb.AddForce(transform.forward * dropForce, ForceMode.VelocityChange);

        isAiming = false;
        isHoldingWeapon = false;
        onDrop?.Invoke();
        currentWeapon = null;

        Debug.Log("Dropped weapon");
    }
}
