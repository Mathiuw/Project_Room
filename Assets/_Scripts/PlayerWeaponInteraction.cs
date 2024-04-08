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

    public event Action<Transform> onShoot;
    public event Action onAimStart;
    public event Action onAimEnd;
    public event Action<Weapon> onPickupEnd;
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
        Weapon = weapon.GetComponent<Weapon>();

        if (Weapon.holder != null)
        {
            base.Weapon = null;
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        //Add components
        Weapon.AddComponent<WeaponAnimationManager>();

        //Set weapon hold state
        Weapon.SetHoldState(true, transform);
        
        //Lerp weapon to player
        StartCoroutine(LerpWeapon(0.2f, weapon, Vector3.zero, Quaternion.identity, weaponHolder));
        while (isLerping) yield return null;

        isHoldingWeapon = true;
        SetWeaponEvents(true);
        onPickupEnd?.Invoke(base.Weapon);

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
        switch (Weapon.shootType) 
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

        if (InputShoot()) onShoot?.Invoke(cameraTransform);
    }

    void Aim(bool b, Vector3 aimVector)
    {
        isAiming = b;

        StopAllCoroutines();
        StartCoroutine(LerpWeapon(aimTime, Weapon.transform, aimVector, Quaternion.identity));
    }

    void AimTrue()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        Vector3 aimVector = -defautHoldPosition - Weapon.GetAimLocation();

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

        if(Weapon.GetAmmo() == Weapon.GetMaxAmmo()) yield break;

        if (!inventory.HaveItem(Weapon.GetReloadItem())) yield break;
        inventory.RemoveItem(Weapon.GetReloadItem());

        isReloading = true;
        onReloadStart?.Invoke(Weapon.GetMaxAmmo());

        yield return new WaitForSeconds(Weapon.GetReloadTime());

        Weapon.AddAmmo(Weapon.GetMaxAmmo());
        isReloading = false;
        onReloadEnd?.Invoke();

        yield break;
    }

    public override void DropWeapon()
    {
        if (!isHoldingWeapon) return;
        if (isReloading) return;

        Transform currentWeaponTransform = Weapon.transform;

        StopAllCoroutines();
        Weapon.SetHoldState(false, null);
        SetWeaponEvents(false);
        currentWeaponTransform.SetParent(null);
        currentWeaponTransform.localScale = Vector3.one;

        Destroy(Weapon.GetComponent<WeaponAnimationManager>());

        isLerping = false;
        isAiming = false;
        isReloading = false;
        isHoldingWeapon = false;

        onDrop?.Invoke();

        Weapon = null;
        Debug.Log("Dropped weapon");
    }

    void WeaponCameraShake() 
    {
        cameraShake.AddCameraShake(Weapon.GetIntensity(), Weapon.GetSpeed());
    }

    void SetWeaponEvents(bool b) 
    {
        if (!isHoldingWeapon) return;
        
        WeaponAnimationManager weaponAnimationManager = Weapon.GetComponent<WeaponAnimationManager>();
        Animator animator = Weapon.GetComponentInChildren<Animator>();

        animator.enabled = b;

        if (b) 
        {
            onShoot += Weapon.Shoot;
            
            Weapon.onShoot += WeaponCameraShake;
            Weapon.onShoot += weaponAnimationManager.ShootWeaponAnimation;
        }
        else
        {
            onShoot -= Weapon.Shoot;

            Weapon.onShoot -= WeaponCameraShake;
            Weapon.onShoot -= weaponAnimationManager.ShootWeaponAnimation;
        }
    }
}
