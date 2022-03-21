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

    private MeshCollider[] cols;
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
            hit.transform.SetParent(gunHolder);

            Vector3 startPosition = hit.transform.localPosition;
            Quaternion startRotation = hit.transform.localRotation;
            float elapsedTime = 0;
            float waitTime = 0.2f;

            hit.transform.GetComponent<Name>().enabled = false;
            hit.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            hit.rigidbody.isKinematic = true;
            cols = hit.transform.GetComponentsInChildren<MeshCollider>();
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].isTrigger = true;
            }
            hit.transform.GetComponentInChildren<MeshCollider>().isTrigger = true;

            while (hit.transform.localPosition != Vector3.zero && hit.transform.localRotation != Quaternion.identity)
            {
                hit.transform.localPosition = Vector3.Lerp(startPosition, Vector3.zero, elapsedTime / waitTime);
                hit.transform.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, elapsedTime / waitTime);
                hit.transform.localScale = Vector3.one;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            hit.transform.gameObject.GetComponent<ShootGun>().beingHold = true;
            Debug.Log("Picked up gun");
            yield break;
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
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].isTrigger = false;
            }

            animator.Play("Not Holding Weapon");
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