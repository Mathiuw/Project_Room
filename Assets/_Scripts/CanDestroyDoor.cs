using UnityEngine;

public class CanDestroyDoor : MonoBehaviour
{
    [SerializeField] float force;

    void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        DoorDestroy doorDestroy;

        if (doorDestroy = collision.transform.GetComponentInParent<DoorDestroy>()) 
        {
            doorDestroy.DestroyDoor(transform.forward, force);
        }
    }
}
