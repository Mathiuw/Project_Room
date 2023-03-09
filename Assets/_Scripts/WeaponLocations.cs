using UnityEngine;

public class WeaponLocations : MonoBehaviour
{
    [SerializeField] Transform aimLocation;
    [SerializeField] Transform muzzleFlashLocation;

    public Vector3 GetAimLocation() { return aimLocation.localPosition; }

    public Vector3 GetMuzzleFlashLocation() { return muzzleFlashLocation.localPosition; }
}
