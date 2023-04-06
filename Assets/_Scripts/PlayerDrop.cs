using UnityEngine;

[RequireComponent(typeof(PlayerWeaponInteraction))]
public class PlayerDrop : MonoBehaviour
{
    [SerializeField] float dropForce;

    public void Drop(Transform dropTransform)
    {
        Rigidbody dropRigidbody = dropTransform.GetComponent<Rigidbody>();

        dropTransform.localPosition = transform.position;
        dropTransform.rotation = transform.rotation;
        dropRigidbody.AddForce(transform.forward * dropForce, ForceMode.VelocityChange);
    }
}
