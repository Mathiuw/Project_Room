using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponInteraction : MonoBehaviour
{
    public Transform raycastTransform;
    [SerializeField] Transform gunHolder;
    [SerializeField] LayerMask WeaponMask;
    [SerializeField] int maxItemDistance = 5;
    [SerializeField] float dropForce = 10;
    float percentageComplete = 0f;
    public bool isHoldingWeapon { get; private set; } = false;

    RaycastHit hit;
    public weapon currentWeapon { get; protected set; }
    
    public event Action<Transform> onPickupStart;
    public event Action<Transform> onPickupEnd;
    public event Action<Transform> onWeaponDrop;

    void Start() 
    {
        if (gunHolder.childCount != 0) StartCoroutine(PickUpWeapon(gunHolder.GetChild(0)));
    }

    public void TryToPickupWeapon() 
    {
        if (!isHoldingWeapon && Physics.Raycast(raycastTransform.position, raycastTransform.forward, out hit, maxItemDistance, WeaponMask)) 
            StartCoroutine(PickUpWeapon(hit.transform));
    }

    IEnumerator PickUpWeapon(Transform gun)
    {
        currentWeapon = gun.GetComponent<weapon>();

        if (currentWeapon.isBeingHold) 
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        onPickupStart?.Invoke(gun);

        currentWeapon.transform.SetParent(gunHolder);
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.OnBeingHold(true);

        float elapsedTime = 0f;
        float waitTime = 0.2f;
        percentageComplete = 0f;

        Vector3 startPosition = gun.localPosition;
        Quaternion startRotation = gun.localRotation;

        while (elapsedTime < waitTime)
        {
            gun.localPosition = Vector3.Lerp(startPosition, Vector3.zero, percentageComplete);
            gun.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, percentageComplete);

            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / waitTime;
            yield return null;
        }
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;

        isHoldingWeapon = true;
        onPickupEnd?.Invoke(gun);

        Debug.Log("Picked up gun");
        yield break;
    }

    public void DropGun()
    {
        if (!isHoldingWeapon || currentWeapon.reloadGun.reloading) return;

        currentWeapon.OnBeingHold(false);
        currentWeapon.transform.SetParent(null);
        currentWeapon.transform.localScale = Vector3.one;     
        currentWeapon.transform.localPosition = raycastTransform.position + raycastTransform.forward * 1.5f;
        currentWeapon.rb.AddForce(raycastTransform.forward * dropForce, ForceMode.VelocityChange);

        isHoldingWeapon = false;
        onWeaponDrop?.Invoke(currentWeapon.transform);
        currentWeapon = null;

        Debug.Log("Dropped gun");
    }
}