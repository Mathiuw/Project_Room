using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.UIElements;

public class WeaponInteraction : MonoBehaviour
{
    public Transform raycastTransform;
    [SerializeField] Transform gunHolder;
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxItemDistance = 5;
    [SerializeField] float dropForce = 10;
    public bool isHoldingWeapon { get; private set; } = false;

    Collider[] cols;
    RaycastHit hit;
    weapon weapon;

    public event Action<Transform> PickupStarted;
    public event Action<Transform> PickupEnded;
    public event Action<Transform> weaponDropped;

    void Start() 
    {
        if (gunHolder.GetChild(0) != null) StartCoroutine(PickUpCoroutine(gunHolder.GetChild(0)));
    }

    public void WeaponPickup() 
    {
        if (!isHoldingWeapon && Physics.Raycast(raycastTransform.position, raycastTransform.forward, out hit, maxItemDistance, WeaponMask)) 
            StartCoroutine(PickUpCoroutine(hit.transform));
    }

    IEnumerator PickUpCoroutine(Transform gun)
    {
        weapon = gun.GetComponent<weapon>();

        if (weapon.IsBeingHold) 
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        PickupStarted?.Invoke(gun);

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

        isHoldingWeapon = true;
        weapon.BeingHold(true);
        PickupEnded?.Invoke(gun);

        Debug.Log("Picked up gun");
        yield break;
    }

    public void DropGun()
    {
        if (!isHoldingWeapon || weapon.reloadGun.reloading) return;

        weapon.shootGun.ResetGunEvents();
        weapon.weaponName.enabled = true;
        weapon.transform.SetParent(null);
        weapon.transform.localPosition = raycastTransform.position + raycastTransform.forward * 1.5f;

        cols = weapon.GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++) cols[i].isTrigger = false;

        weapon.rb.isKinematic = false;
        weapon.rb.interpolation = RigidbodyInterpolation.Interpolate;
        weapon.rb.AddForce(raycastTransform.forward * dropForce, ForceMode.VelocityChange);

        isHoldingWeapon = false;
        weapon.BeingHold(false);
        weaponDropped?.Invoke(weapon.transform);

        Debug.Log("Dropped gun");
    }
}