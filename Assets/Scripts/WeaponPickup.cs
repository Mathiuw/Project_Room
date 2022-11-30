using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform Camera;
    [SerializeField] private int maxItemDistance = 5;
    [SerializeField] private float dropForce = 10;

    private Collider[] cols;
    private RaycastHit hit;

    public event Action<Transform> onPickupCoroutineStart;
    public event Action<Transform> onPickupCoroutineEnd;
    public event Action<Transform> weaponDrop;

    void start() => HaveGunCheck();

    public void PickupRaycast() 
    {
        if (gunHolder.childCount == 0 && Physics.Raycast(Camera.position, Camera.forward, out hit, maxItemDistance) &&
            hit.transform.GetComponentInParent<ShootGun>()) PickupGun(hit.transform);
    }

    void PickupGun(Transform gun) => StartCoroutine(PickUpCoroutine(gun));

    IEnumerator PickUpCoroutine(Transform gun)
    {
        weapon weapon = gun.GetComponent<weapon>();

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
        weapon.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
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
        onPickupCoroutineEnd?.Invoke(hit.transform);
        weapon.BeingHold(true);

        Debug.Log("Picked up gun");
        yield break;
    }

    public void DropGun(Transform gun)
    {
        weapon weapon = gun.GetComponent<weapon>();

        if (IsholdingWeapon() && !weapon.reloadGun.reloading)
        {
            weapon.shootGun.ResetGunEvents();
            weapon.weaponName.enabled = true;  
            gun.SetParent(null);
            gun.localPosition = Camera.position + Camera.forward * 1.5f;

            cols = gun.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++) cols[i].isTrigger = false;

            weapon.rb.isKinematic = false;
            weapon.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            weapon.rb.AddForce(Camera.forward * dropForce, ForceMode.VelocityChange);
            weaponDrop?.Invoke(gun);

            weapon.BeingHold(false);

            Debug.Log("Dropped gun");
        }
    }

    void HaveGunCheck() 
    {
        if (gunHolder.TryGetComponent(out ShootGun shootGun)) PickupGun(shootGun.transform);
    }

    public bool IsholdingWeapon()
    {
        if (gunHolder.childCount > 0) return true;
        return false;
    }
}