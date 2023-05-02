using UnityEngine;

public class WeaponLocations : MonoBehaviour
{
    [SerializeField] Transform aimLocation;
    [SerializeField] Transform muzzleFlashLocation;
    [SerializeField] Transform ammoMeshTransform;

    public Vector3 GetAimLocation() { return aimLocation.localPosition; }

    public Vector3 GetMuzzleFlashLocation() { return muzzleFlashLocation.localPosition; }

    public Transform GetAmmoMeshTransform() { return ammoMeshTransform; }
}
