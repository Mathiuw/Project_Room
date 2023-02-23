using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerWeaponInteraction : WeaponInteraction
{
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxPickupDistance = 5;
    [SerializeField] float dropForce = 10;
    float percentageComplete = 0f;

    public bool isAiming { get; private set; } = false;

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

        if (Input.GetKeyDown(KeyCode.Mouse1)) Aim();
        else if(Input.GetKeyUp(KeyCode.Mouse1)) Aim();

        if (Input.GetKeyDown(KeyCode.E)) TryToPickupWeapon();

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

        if (Input.GetKeyDown(KeyCode.G)) DropGun();
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
        currentWeapon.shootGun.Shooting(cameraTransform);
    }

    void Aim() 
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        isAiming = !isAiming;

        if(isAiming) onAimStart?.Invoke();
        else onAimEnd?.Invoke();
    }

    protected override IEnumerator PickUpWeapon(Transform gun)
    {
        if (isHoldingWeapon) yield break;

        enabled = false;

        currentWeapon = gun.GetComponent<weapon>();

        if (currentWeapon.isBeingHold)
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        onPickupStart?.Invoke(gun);

        gun.SetParent(gunHolder);
        gun.localScale = Vector3.one;
        currentWeapon.OnBeingHold(true);

        float elapsedTime = 0f;
        float waitTime = 0.2f;
        percentageComplete = 0f;

        Vector3 startPosition = gun.localPosition;
        Quaternion startRotation = gun.localRotation;
        while (elapsedTime < waitTime)
        {
            gun.localPosition = Vector3.Lerp(startPosition, Vector3.zero, percentageComplete);
            gun.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, percentageComplete);

            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / waitTime;
            yield return null;
        }
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;

        isHoldingWeapon = true;
        enabled = true;
        onPickupEnd?.Invoke(gun);

        Debug.Log("Picked up gun");
        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!isHoldingWeapon) yield break;

        ReloadGun reloadGun = currentWeapon.reloadGun;
        reloadGun.onReloadStart += UI_Inventory.instance.RefreshInventory;

        StartCoroutine(reloadGun.Reload(reloadGun.reloadTime, inventory));
        yield break;
    }

    public override void DropGun()
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        currentWeapon.OnBeingHold(false);
        currentWeapon.transform.SetParent(null);
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.transform.localPosition = cameraTransform.position + cameraTransform.forward * 1.5f;
        currentWeapon.rb.AddForce(cameraTransform.forward * dropForce, ForceMode.VelocityChange);

        isHoldingWeapon = false;
        onDrop?.Invoke();
        currentWeapon = null;

        Debug.Log("Dropped gun");
    }
}
