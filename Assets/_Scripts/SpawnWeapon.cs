using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{
    [SerializeField] SOWeapon weaponSO;

    void Awake() 
    {
        if (weaponSO == null)
        {
            Debug.LogError("Weapon SO is Null");
            return;
        }

        SpawnMesh();
        SetWeaponComponents();
        Destroy(this);
    }

    public void SpawnMesh() 
    {
        //GameObject model = Instantiate(weaponSO.Model, transform);

        //model.transform.localPosition = Vector3.zero;
        //model.transform.localRotation = Quaternion.identity;
    }

    void SetWeaponComponents() 
    {
        //weapon name
        Name weaponName = gameObject.AddComponent<Name>();
        name = weaponSO.weaponName;
        weaponName.SetText(weaponSO.weaponName);

        //weapon animator
        Animator animator = GetComponentInChildren<Animator>();
        if (weaponSO.animatorOverride != null) animator.runtimeAnimatorController= weaponSO.animatorOverride;
        animator.enabled = false;

        //weapon muzzle flash
        GameObject muzzleFlash = Instantiate(weaponSO.muzzleFlash, transform);
        muzzleFlash.transform.localPosition= GetComponent<Weapon>().GetMuzzleFlashLocation();
        muzzleFlash.transform.localRotation = weaponSO.muzzleFlash.transform.rotation;

        //weapon audio
        GetComponent<AudioSource>().clip = weaponSO.shootAudio;
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.localPosition, transform.localPosition + transform.up * .5f);
        Gizmos.DrawLine(transform.localPosition, transform.localPosition + -transform.right * .5f);
    }
}
