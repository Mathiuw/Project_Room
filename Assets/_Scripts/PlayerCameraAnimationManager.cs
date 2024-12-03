using UnityEngine;

public class PlayerCameraAnimationManager : MonoBehaviour
{
    PlayerWeaponInteraction playerWeaponInteraction;
    PlayerMovement playerMovement;
    Animator animator;
    Rigidbody rb;

    void Awake() 
    { 
        animator = GetComponent<Animator>(); 
    }

    void Start() 
    {
        Player player = FindFirstObjectByType<Player>();

        playerWeaponInteraction = player.GetComponent<PlayerWeaponInteraction>();
        playerMovement = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();

        playerWeaponInteraction.onWeaponPickup += OnPickup;
        playerWeaponInteraction.onWeaponDrop += OnDrop;
        playerWeaponInteraction.onAimStart += ActivateAim;

    }

    void Update() 
    {
        animator.SetFloat("Walk Speed", WalkSpeed());
        animator.SetFloat("RbVelocity", rb.linearVelocity.magnitude);
        animator.SetBool("Hold", playerWeaponInteraction.GetIsHoldingWeapon());
        
        if (playerWeaponInteraction.GetIsHoldingWeapon()) 
        {
            animator.SetBool("Aim", playerWeaponInteraction.GetIsAiming());
            animator.SetBool("Reload", playerWeaponInteraction.GetIsReloading());
        } 
    } 

    void ReloadStart(float duration) => animator.Play("Start Reload");

    void ReloadEnd() => animator.Play("End Reload");

    void OnPickup(Weapon weaponPicked) 
    {
        playerWeaponInteraction.onReloadStart += ReloadStart;
        playerWeaponInteraction.onReloadEnd += ReloadEnd;
    } 

    void OnDrop() 
    {
        playerWeaponInteraction.onReloadStart -= ReloadStart;
        playerWeaponInteraction.onReloadEnd -= ReloadEnd;

        animator.Rebind();
    }

    void ActivateAim() 
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !animator.GetCurrentAnimatorStateInfo(0).IsName("AimTrue") ) 
            animator.Play("Idle");
    }

    float WalkSpeed() 
    {
        if (playerMovement.GetIsSprinting()) return 1.5f;
        else return 1f;
    }
}
