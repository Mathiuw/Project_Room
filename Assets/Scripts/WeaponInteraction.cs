using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.UIElements;

public class WeaponInteraction : MonoBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxItemDistance = 5;
    [SerializeField] float dropForce = 10;
    [HideInInspector] public Transform raycastTransform;
    Collider[] cols;
    RaycastHit hit;
    weapon weapon;

    public event Action<Transform> onPickupCoroutineStart;
    public event Action<Transform> onPickupCoroutineEnd;
    public event Action<Transform> weaponDrop;

    public void WeaponPickup() 
    {
        if (!IsholdingWeapon() && Physics.Raycast(raycastTransform.position, raycastTransform.forward, out hit, maxItemDistance, WeaponMask)) 
            StartCoroutine(PickUpCoroutine(hit.transform));
    }

    IEnumerator PickUpCoroutine(Transform gun)
    {
        weapon = gun.GetComponent<weapon>();

        if (weapon.IsBeingHold == true) 
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        onPickupCoroutineStart?.Invoke(gun);

        gun.SetParent(gunHolder);

        Vector3 startPosition = gun.localPosition;
        Quaternion startRotation = gun.localRotation;

        float elapsedTime = 0;
        float waitTime = 0.2f;
        
        weapon.weaponName.enabled = false;
        weapon.rb.interpolation = RigidbodyInterpolation.None;
        weapon.rb.isKinematic = true;

        cols = gun.GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++) cols[i].isTrigger = true;

        while (gun.localPosition != Vector3.zero && gun.localRotation != Quaternion.identity)
        {
            gun.localPosition = Vector3.Lerp(startPosition, Vector3.zero, elapsedTime / waitTime);
            gun.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, elapsedTime / waitTime);
            gun.localScale = Vector3.one;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weapon.shootGun.onHit += UI_Hit.Instance.OnHit;

        onPickupCoroutineEnd?.Invoke(gun);

        weapon.BeingHold(true);

        Debug.Log("Picked up gun");
        yield break;
    }

    public void DropGun()
    {
        if (!IsholdingWeapon()) return;

        if (IsholdingWeapon() && !weapon.reloadGun.reloading)
        {
            weapon.shootGun.ResetGunEvents();
            weapon.weaponName.enabled = true;  
            weapon.transform.SetParent(null);
            weapon.transform.localPosition = raycastTransform.position + raycastTransform.forward * 1.5f;

            cols = weapon.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++) cols[i].isTrigger = false;

            weapon.rb.isKinematic = false;
            weapon.rb.interpolation = RigidbodyInterpolation.Interpolate;
            weapon.rb.AddForce(raycastTransform.forward * dropForce, ForceMode.VelocityChange);

            weaponDrop?.Invoke(weapon.transform);

            weapon.BeingHold(false);

            Debug.Log("Dropped gun");
        }
    }

    public bool IsholdingWeapon()
    {
        if (gunHolder.childCount > 0) return true;
        return false;
    }
}