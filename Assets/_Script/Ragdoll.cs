using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] bool ActivateOnStart = false;
   
    void Start() 
    {
        if (ActivateOnStart) RagdollActive(true);    
        else RagdollActive(false);
    }   

    //Ativa ou desativa os rigidbodies
    public void RagdollActive(bool b) 
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()) 
        {
            rb.isKinematic = !b;
        }

        foreach (Collider collider in GetComponentsInChildren<Collider>()) 
        {
            collider.isTrigger = !b;
        }

        GetComponent<Collider>().isTrigger = b;

        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> ragdoll = <b><color=cyan>" + b + "</color></b>");
    }
}
