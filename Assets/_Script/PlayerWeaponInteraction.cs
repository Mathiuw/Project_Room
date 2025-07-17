using System;
using System.Collections;
using UnityEngine;

public class PlayerWeaponInteraction : WeaponInteraction
{
    [Header("Weapon Interation")]
    [SerializeField] float aimTime;

    [Header("Weapon Sway")]
    [SerializeField] float smooth = 8;
    [SerializeField] float swayMultiplier = 4;

    public bool IsReloading { get; private set; } = false;
    public bool isLerping { get; private set; } = false;

    Transform cameraTransform;
    Inventory inventory;

    public event Action onWeaponShot;
    public event Action<Weapon> onWeaponPickup;
    public event Action<float> onReloadStart;
    public event Action onReloadEnd;
    public event Action onWeaponDrop;

    void Awake() 
    {
        inventory = GetComponent<Inventory>();

        if (!inventory)
        {
            Debug.LogWarning("Cant find inventory");
        }
    } 

    void Start() 
    {
        cameraTransform = Camera.main.transform;

        CameraMovement playerCamera = FindAnyObjectByType<CameraMovement>();

        if (playerCamera) 
        {
            weaponContainer = playerCamera.WeaponHolder;
        } 
    } 

    void Update()
    {
        Shoot();

        //if (Input.GetKeyDown(KeyCode.Mouse1)) EnableAim();
        //else if(Input.GetKeyUp(KeyCode.Mouse1)) StopAim();

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

        if (Input.GetKeyDown(KeyCode.G)) DropWeapon();

        // Weapon sway
        if (Weapon) 
        {
            SwayWeapon(swayMultiplier);
        }
    }

    IEnumerator LerpWeaponCoroutine(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation, Transform parent = null) 
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

    void SwayWeapon(float swayMultiplier)
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        Weapon.transform.localRotation = Quaternion.Slerp(Weapon.transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    public override IEnumerator PickUpWeapon(Weapon pickedWeapon)
    {
        if (Weapon) yield break;
        if (isLerping) yield break;
        if (pickedWeapon.owner != null)
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        //Set current weapon
        Weapon = pickedWeapon;

        //Set weapon hold state
        Weapon.SetHoldState(true, transform);
        
        //Lerp weapon to player
        StartCoroutine(LerpWeaponCoroutine(0.2f, pickedWeapon.transform, Vector3.zero, Quaternion.identity, weaponContainer));
        while (isLerping) yield return null;

        //SetWeaponEvents(true);
        onWeaponPickup?.Invoke(base.Weapon);

        Debug.Log("Picked up weapon");
    }

    bool InputShoot() 
    {
        switch (Weapon.SOWeapon.shootType)
        {
            case EShootType.Single:
                return Input.GetKeyDown(KeyCode.Mouse0);
            case EShootType.Automatic:
                return Input.GetKey(KeyCode.Mouse0);
            default: 
                return false;
        }
    }

    void Shoot()
    {
        if (!Weapon) return;
        if (IsReloading) return;
        if (isLerping) return;

        if (InputShoot()) 
        {
            Weapon.Shoot(cameraTransform);
            onWeaponShot?.Invoke();
        } ;
    }

    public override IEnumerator ReloadWeapon()  
    {
        if (!Weapon) yield break;
        if (IsReloading) yield break;
        if(Weapon.Ammo == Weapon.Ammo) yield break;
        if (inventory == null) yield break;
        //if (!inventory.HaveItem(weapon.GetReloadItem())) yield break;
        if (inventory.GetAmmoAmountByType(Weapon.SOWeapon.ammoType) == 0)
        {
            Debug.Log("No ammo to reload");
            yield break;
        }

        IsReloading = true;

        yield return new WaitForSeconds(Weapon.SOWeapon.reloadTime);

        // Old reloading logic
        //inventory.RemoveItem(weapon.GetReloadItem());
        //onReloadStart?.Invoke(weapon.GetMaxAmmo());

        // New reloading logic
        // Remove ammo from player inventory
        EAmmoType ammoType = Weapon.SOWeapon.ammoType;
        int amountToReload = 0;
        int inventoryAmount = inventory.GetAmmoAmountByType(ammoType);

        for (int i = Weapon.Ammo; i <= Weapon.SOWeapon.maxAmmo; i++)
        {
            if (inventoryAmount == 0)
            {
                break;
            }
            inventoryAmount--;

            amountToReload++;
        }


        inventory.RemoveAmmo(ammoType, amountToReload);

        //Add ammo to weapon
        Weapon.AddAmmo(amountToReload);

        IsReloading = false;
        onReloadEnd?.Invoke();

        yield break;
    }

    public override void DropWeapon()
    {
        if (!Weapon) return;
        if (IsReloading) return;

        StopAllCoroutines();
        onWeaponDrop?.Invoke();

        Transform weaponTransform = Weapon.transform;
        Rigidbody weaponRb = weaponTransform.GetComponent<Rigidbody>();

        Weapon.SetHoldState(false, null);
        weaponTransform.SetParent(null);
        weaponTransform.transform.position = transform.position;
        weaponRb.AddForce(transform.forward * 5, ForceMode.VelocityChange);
        weaponTransform.localScale = Vector3.one;

        isLerping = false;
        IsReloading = false;

        Weapon = null;

        Debug.Log("Dropped weapon");
    }
}
