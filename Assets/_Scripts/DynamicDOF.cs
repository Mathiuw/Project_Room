using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Animations;

public class DynamicDOF : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] float focusSpeed;
    [SerializeField] LayerMask collsionMask;
    float maxDistance = 5f;
    float distance;
    PostProcessVolume postProcessVolume;
    DepthOfField dof;

    void Start()
    {
        postProcessVolume = GameObject.FindGameObjectWithTag("Post Processing").transform.GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out dof);
    }

    void Update()
    {
        RaycastHit hit;
        
        bool isAiming = transform.root.GetComponent<Animator>().GetBool("isAiming");

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, collsionMask) && !isAiming)
        {
            distance = Vector3.Distance(transform.position, hit.point);
        }
        else distance = maxDistance;

        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, distance, focusSpeed * Time.deltaTime);
    }
}
