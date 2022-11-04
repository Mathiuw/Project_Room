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
        Name gunName = gun.GetComponent<Name>();
        Rigidbody GunRB = gun.GetComponent<Rigidbody>();

        onPickupCoroutineStart?.Invoke(gun);
        gun.SetParent(gunHolder);

        Vector3 startPosition = gun.localPosition;
        Quaternion startRotation = gun.localRotation;

        float elapsedTime = 0;
        float waitTime = 0.2f;

        gunName.enabled = false;
        GunRB.collisionDetectionMode = CollisionDetectionMode.Discrete;
        GunRB.isKinematic = true;

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

        gun.GetComponent<ShootGun>().beingHold = true;
        onPickupCoroutineEnd?.Invoke(hit.transform);

        Debug.Log("Picked up gun");
        yield break;
    }

    public void DropGun(Transform gun)
    {
        ShootGun shootGun = gun.GetComponent<ShootGun>();
        ReloadGun reloadGun = gun.GetComponent<ReloadGun>();
        Rigidbody GunRB = gun.GetComponent<Rigidbody>();

        if (IsholdingWeapon() && !reloadGun.reloading)
        {
            shootGun.beingHold = false;
            shootGun.ResetGunEvents();
            gun.GetComponent<Name>().enabled = true;  
            gun.SetParent(null);
            gun.localPosition = Camera.position + Camera.forward * 1.5f;

            cols = gun.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++) cols[i].isTrigger = false;

            GunRB.isKinematic = false;
            GunRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            GunRB.AddForce(Camera.forward * dropForce, ForceMode.VelocityChange);

            weaponDrop?.Invoke(gun);

            Debug.Log("Dropped gun");
        }
    }

    void HaveGunCheck() 
    {
        if (TryGetComponent(out ShootGun shootGun)) PickupGun(shootGun.transform);
    }

    public bool IsholdingWeapon()
    {
        if (gunHolder.childCount > 0) return true;
        return false;
    }
}