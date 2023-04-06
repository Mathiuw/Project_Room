using UnityEngine;

public class DoorDestroy : MonoBehaviour
{
    [SerializeField] Transform[] doors;

    public void DestroyDoor(Vector3 direction, float force) 
    {
        Name nameScript;
        Door doorScript;

        if (doorScript = GetComponent<Door>()) 
        {
            if (doorScript.open) return;
            Destroy(doorScript);
        } 
        if (nameScript = GetComponent<Name>()) Destroy(nameScript);

        foreach (Transform door in doors)
        {
            door.SetParent(null);

            Rigidbody doorRB = door.GetComponentInChildren<Rigidbody>();

            doorRB.isKinematic = false;
            doorRB.interpolation = RigidbodyInterpolation.Interpolate;            
            doorRB.AddForce(direction * force, ForceMode.VelocityChange);
            doorRB.AddTorque(direction * force, ForceMode.VelocityChange);
        }
    }
}
