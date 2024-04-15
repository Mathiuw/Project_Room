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
        Player player = FindObjectOfType<Player>();

        playerWeaponInteraction = player.GetComponent<PlayerWeaponInteraction>();
        sprint= player.GetComponent<Sprint>();
        rb = player.GetComponent<Rigidbody>();

        playerWeaponInteraction.onPickupWeapon += OnPickup;
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
            animator.SetBool("Reload", playerWeaponInteraction.isReloading);
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
        if (sprint.isRunning) return 1.5f;
        else return 1f;
    }
}
