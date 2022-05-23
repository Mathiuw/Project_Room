using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Animations;

public class DynamicDOF : MonoBehaviour
{
    [SerializeField] float focusSpeed;
    float maxDistance = 5f;
    float distance;
    PostProcessVolume postProcessVolume;

    void Awake()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
    }

    void Update()
    {
        RaycastHit hit;
        DepthOfField dof;
        bool isAiming = transform.root.GetComponent<Animator>().GetBool("isAiming");
        
        postProcessVolume.profile.TryGetSettings<DepthOfField>(out dof);
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance) && !isAiming)
        {
            distance = Vector3.Distance(transform.position, hit.point);
        }
        else distance = maxDistance;
        dof.focusDistance.value = distance;//Mathf.Lerp(dof.focusDistance.value, distance, Time.deltaTime * focusSpeed);
    }
}
