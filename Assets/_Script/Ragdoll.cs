using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] bool _activateOnStart = false;
   
    void Start() 
    {
        if (_activateOnStart) RagdollState(true);
        else RagdollState(false);
    }   

    public void RagdollState(bool b) 
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()) 
        {
            rb.isKinematic = !b;
        }

        foreach (Collider collider in GetComponentsInChildren<Collider>()) 
        {
            collider.isTrigger = !b;
        }
    }
}
