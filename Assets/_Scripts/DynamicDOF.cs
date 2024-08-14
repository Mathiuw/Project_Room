using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DynamicDOF : MonoBehaviour
{
    [Range(1,10)]
    [SerializeField] float focusSpeed = 7.5f;
    [SerializeField] LayerMask collsionMask;
    float maxDistance = 5f;
    float distance;
    PostProcessVolume postProcessVolume;
    DepthOfField dof;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Post Processing"))
        {
            postProcessVolume = GameObject.FindGameObjectWithTag("Post Processing").transform.GetComponent<PostProcessVolume>();
            postProcessVolume.profile.TryGetSettings(out dof);
        }
        else 
        {
            Debug.LogError("Cant find PostProcessVolume, disabling component");
            enabled = false;
        }
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, collsionMask))
        {
            distance = Vector3.Distance(transform.position, hit.point);
        }
        else distance = maxDistance;

        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, distance, focusSpeed * Time.deltaTime);
    }
}
