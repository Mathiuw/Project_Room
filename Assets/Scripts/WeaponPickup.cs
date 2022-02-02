using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private static Transform gunHolder;
    [SerializeField] private Transform Camera;
    [SerializeField] private int maxItemDistance = 5;
    [SerializeField] private float dropForce = 10;

    private MeshCollider[] cols;
    private RaycastHit hit;

    private void Awake()
    {
        gunHolder = GameObject.Find("Gun_holder").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpGun();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropGun();
        }
    }

    private void PickUpGun()
    {
        if (!IsHoldingWeapon())
        {
            if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxItemDistance) && hit.transform.tag == "Gun")
            {
                hit.transform.gameObject.GetComponent<ShootGun>().beingHold = true;
                hit.transform.GetComponent<Name>().enabled = false;
                hit.transform.SetParent(gunHolder);
                hit.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                hit.rigidbody.isKinematic = true;
                cols = hit.transform.GetComponentsInChildren<MeshCollider>();
                for (int i = 0; i < cols.Length; i++)
                {
                    cols[i].isTrigger = true;
                }
                hit.transform.GetComponentInChildren<MeshCollider>().isTrigger = true;
                hit.transform.localPosition = Vector3.zero;
                hit.transform.localRotation = Quaternion.Euler(Vector3.zero);
                Debug.Log("Picked up gun");
            }
        }
    }

    private void DropGun()
    {
        if (IsHoldingWeapon())
        {
            hit.transform.gameObject.GetComponent<ShootGun>().beingHold = false;
            hit.transform.GetComponent<Name>().enabled = true;
            hit.transform.SetParent(null);
            hit.transform.localPosition = Camera.position + Camera.forward * 1.5f;
            hit.rigidbody.isKinematic = false;
            hit.rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            cols = hit.transform.GetComponentsInChildren<MeshCollider>();
            SceneManager.MoveGameObjectToScene(hit.transform.gameObject, SceneManager.GetActiveScene());
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].isTrigger = false;
            }
            hit.rigidbody.AddForce(Camera.forward * dropForce, ForceMode.VelocityChange);
            Debug.Log("Dropped gun");
        }
    }

    public static bool IsHoldingWeapon()
    {
        if (gunHolder.childCount > 0)
        {
            return true;
        }
        else
            return false;
    }
}