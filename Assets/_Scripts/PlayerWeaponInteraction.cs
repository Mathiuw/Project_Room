using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerWeaponInteraction : WeaponInteraction
{
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxPickupDistance = 5;
    [SerializeField] float dropForce = 10;

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

        if (Input.GetKeyDown(KeyCode.Mouse1)) Aim();
        else if(Input.GetKeyUp(KeyCode.Mouse1)) Aim();

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

    void SetAimPositon(Transform weapon, Vector3 aimPosition) 
    {
        weapon.localPosition = aimPosition; 
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

    void Aim()
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        isAiming = !isAiming;

        if (isAiming)
        {
            StartCoroutine(LerpWeapon(0.3f, currentWeapon.transform, currentWeapon.GetAimVector(), Quaternion.identity));
            onAimStart?.Invoke();
        }
        else
        {
            StartCoroutine(LerpWeapon(0.3f, currentWeapon.transform, Vector3.zero, Quaternion.identity));
            onAimEnd?.Invoke();
        }
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
        currentWeapon.SetHoldState(true);
        onAimStart += currentWeapon.SetAim;
        onAimEnd += currentWeapon.SetAim;

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

        ReloadGun reloadGun = currentWeapon.reloadGun;
        reloadGun.onReloadStart += UI_Inventory.instance.RefreshInventory;

        StartCoroutine(reloadGun.Reload(reloadGun.reloadTime, inventory));
        yield break;
    }

    public override void DropGun()
    {
        if (!isHoldingWeapon) return;
        if (currentWeapon.reloadGun.isReloading) return;

        currentWeapon.SetHoldState(false);
        currentWeapon.transform.SetParent(null);
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.transform.localPosition = cameraTransform.position + cameraTransform.forward * 1.5f;
        currentWeapon.rb.AddForce(cameraTransform.forward * dropForce, ForceMode.VelocityChange);

        onAimStart -= currentWeapon.SetAim;
        onAimEnd -= currentWeapon.SetAim;

        isAiming = false;
        isHoldingWeapon = false;
        onDrop?.Invoke();
        currentWeapon = null;

        Debug.Log("Dropped weapon");
    }
}
