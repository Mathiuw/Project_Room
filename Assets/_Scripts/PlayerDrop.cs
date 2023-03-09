using UnityEngine;

[RequireComponent(typeof(PlayerWeaponInteraction))]
public class PlayerDrop : MonoBehaviour
{
    [SerializeField] float dropForce;

    void Start() 
    {
        GetComponent<PlayerWeaponInteraction>().onDrop += Drop;
        GetComponent<PlayerItemInteraction>().onDrop += Drop;
    }

    void Drop(Transform dropTransform) 
    {
        Rigidbody dropRigidbody = dropTransform.GetComponent<Rigidbody>();

        dropTransform.localPosition = transform.position + transform.forward * 1.5f;
        dropTransform.rotation = transform.rotation;
        dropRigidbody.AddForce(transform.forward * dropForce, ForceMode.VelocityChange);
    }
}
