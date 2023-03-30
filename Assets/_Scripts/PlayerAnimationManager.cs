using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    PlayerWeaponInteraction playerWeaponInteraction;
    Animator animator;
    Sprint sprint;
    Rigidbody rb;

    void Awake() { animator = GetComponent<Animator>(); }

    void Start() 
    {
        if (Player.instance != null) 
        {
            playerWeaponInteraction = Player.instance.GetComponent<PlayerWeaponInteraction>();
            sprint= Player.instance.GetComponent<Sprint>();
            rb = Player.instance.GetComponent<Rigidbody>();
        } 
        else enabled = false;

        playerWeaponInteraction.onPickupEnd += OnPickup;
        playerWeaponInteraction.onDrop += OnDrop;
        playerWeaponInteraction.onAimStart += ActivateAim;
    }

    void Update() 
    {
        animator.SetFloat("Walk Speed", WalkSpeed());
        animator.SetFloat("RbVelocity", rb.velocity.magnitude);
        animator.SetBool("Hold", playerWeaponInteraction.isHoldingWeapon);
        
        if (playerWeaponInteraction.isHoldingWeapon) 
        {
            animator.SetBool("Aim", playerWeaponInteraction.isAiming);
            animator.SetBool("Reload", playerWeaponInteraction.currentWeapon.reloadGun.isReloading);
        } 
    } 

    void ReloadStart() => animator.Play("Start Reload");

    void ReloadEnd() => animator.Play("End Reload");

    void OnPickup(Transform gun) 
    {
        WeaponReload reloadGun = gun.GetComponent<WeaponReload>();

        reloadGun.onReloadStart += ReloadStart;
        reloadGun.onReloadEnd += ReloadEnd;
    } 

    void OnDrop(Transform weapon) 
    {
        WeaponReload reloadGun = weapon.GetComponent<WeaponReload>();

        reloadGun.onReloadStart -= ReloadStart;
        reloadGun.onReloadEnd -= ReloadEnd;

        animator.Rebind();
    }

    void ActivateAim() 
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !animator.GetCurrentAnimatorStateInfo(0).IsName("AimTrue") ) 
            animator.Play("Idle");
    }

    float WalkSpeed() 
    {
        if (sprint.isRunning) return 1.5f;
        else return 1f;
    }
}
