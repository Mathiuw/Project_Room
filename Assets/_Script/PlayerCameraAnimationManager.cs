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
        playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction)
        {
            playerWeaponInteraction.onWeaponPickup += OnPickup;
            playerWeaponInteraction.onWeaponDrop += OnDrop;

            playerMovement = playerWeaponInteraction.GetComponent<PlayerMovement>();
            rb = playerWeaponInteraction.GetComponent<Rigidbody>();
        }
        else 
        {
            Debug.LogError("Cant find player");
            enabled = false;
        }
    }

    void Update() 
    {
        animator.SetFloat("Walk Speed", WalkSpeed());
        animator.SetFloat("RbVelocity", rb.linearVelocity.magnitude);
        animator.SetBool("Hold", playerWeaponInteraction.Weapon);
        
        if (playerWeaponInteraction.Weapon) 
        {
            animator.SetBool("Reload", playerWeaponInteraction.IsReloading);
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
        if (playerMovement && playerMovement.GetIsSprinting()) return 1.5f;
        else return 1f;
    }
}
