using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponPickup : MonoBehaviour,ICanDo
{
    private bool canDo = true;

    [SerializeField] private static Transform gunHolder;
    [SerializeField] private Transform Camera;
    [SerializeField] private int maxItemDistance = 5;
    [SerializeField] private float dropForce = 10;
    private Animator animator;

    private Collider[] cols;
    private RaycastHit hit;

    void Awake()
    {
        gunHolder = GameObject.Find("Gun_holder").transform;

        animator = GetComponentInParent<Animator>();

        FindObjectOfType<Pause>().changePauseState += CheckIfCanDo;
    }

    void Update()
    {
        if (!canDo) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PickUpGun());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropGun();
        }
    }

    private IEnumerator PickUpGun()
    {
        if (!IsHoldingWeapon() && Physics.Raycast(Camera.position, Camera.forward, out hit, maxItemDistance) && hit.transform.GetComponentInParent<ShootGun>())
        {
            Transform gun = hit.transform;
            Name gunName = gun.GetComponent<Name>();

            gun.SetParent(gunHolder);

            Vector3 startPosition = gun.localPosition;
            Quaternion startRotation = gun.localRotation;
            float elapsedTime = 0;
            float waitTime = 0.2f;

            gunName.enabled = false;
            animator.SetTrigger(gunName.text);
            hit.rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            hit.rigidbody.isKinematic = true;

            cols = gun.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].isTrigger = true;
            }

            while (gun.localPosition != Vector3.zero && gun.localRotation != Quaternion.identity)
            {
                gun.localPosition = Vector3.Lerp(startPosition, Vector3.zero, elapsedTime / waitTime);
                gun.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, elapsedTime / waitTime);
                gun.localScale = Vector3.one;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gun.GetComponent<ShootGun>().beingHold = true;
            Debug.Log("Picked up gun");
            yield break;
        }
    }

    private void DropGun()
    {
        if (IsHoldingWeapon())
        {
            Transform gun = hit.transform;

            gun.gameObject.GetComponent<ShootGun>().beingHold = false;
            gun.GetComponent<Name>().enabled = true;
            gun.SetParent(null);
            gun.localPosition = Camera.position + Camera.forward * 1.5f;

            cols = gun.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].isTrigger = false;
            }

            hit.rigidbody.isKinematic = false;
            hit.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            hit.rigidbody.AddForce(Camera.forward * dropForce, ForceMode.VelocityChange);

            animator.SetBool("isAiming",false);
            animator.SetBool("isShooting",false);
            animator.ResetTrigger("ReloadEnd");
            animator.Play("Not Holding Weapon");

            Debug.Log("Dropped gun");
        }
    }

    public static bool IsHoldingWeapon()
    {
        if (gunHolder.childCount > 0)
        {
            return true;
        }
        else return false;
    }

    public void CheckIfCanDo(bool check)
    {
        if (check)
        {
            canDo = false;
        }
        else canDo = true;
    }
}