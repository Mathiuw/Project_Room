using UnityEngine;

public class PlayerWeaponSway : MonoBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;
    PlayerWeaponInteraction playerWeaponInteraction;

    void Start() 
    {
        playerWeaponInteraction = FindObjectOfType<Player>().GetComponent<PlayerWeaponInteraction>();

        if (Pause.instance != null) Pause.instance.onPause += OnPause;
    }

    void Update() 
    {
        if (!playerWeaponInteraction.isHoldingWeapon) return;

        Transform weapon = playerWeaponInteraction.Weapon.transform;

        if (playerWeaponInteraction.isAiming) Sway(weapon, swayMultiplier / 20);
        else Sway(weapon, swayMultiplier);
    } 

    void Sway(Transform weapon, float swayMultiplier)
    {       
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    void OnPause(bool b) => enabled = !b;
}
