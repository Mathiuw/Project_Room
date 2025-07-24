using UnityEngine;

public class PlayerCameraAnimationManager : MonoBehaviour
{
    PlayerWeaponInteraction playerWeaponInteraction;
    PlayerMovement playerMovement;
    Animator animator;
    Rigidbody playerRb;

    void Awake() 
    { 
        animator = GetComponent<Animator>(); 
    }

    void Start() 
    {
        playerWeaponInteraction = FindFirstObjectByType<PlayerWeaponInteraction>();

        if (playerWeaponInteraction)
        {
            playerWeaponInteraction.onWeaponDrop += OnDrop;
            playerWeaponInteraction.onReloadStart += ReloadStart;
            playerWeaponInteraction.onReloadEnd += ReloadEnd;

            playerMovement = playerWeaponInteraction.GetComponent<PlayerMovement>();
            playerRb = playerWeaponInteraction.GetComponent<Rigidbody>();
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
        animator.SetFloat("RbVelocity", playerRb.linearVelocity.magnitude);
        animator.SetBool("Hold", playerWeaponInteraction.Weapon);
        
        if (playerWeaponInteraction.Weapon) 
        {
            animator.SetBool("Reload", playerWeaponInteraction.IsReloading);
        } 
    } 

    void ReloadStart() => animator.Play("Start Reload");

    void ReloadEnd() => animator.Play("End Reload");

    void OnDrop() => animator.Rebind();

    float WalkSpeed() 
    {
        if (playerMovement && playerMovement.IsSprinting) return 1.5f;
        else return 1f;
    }
}
