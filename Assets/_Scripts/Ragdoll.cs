using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] bool ActivateOnStart = false;
    Rigidbody[] rbs;
   
    void Start() 
    {
        //Pega todos os rigidbodies na herança
        rbs = GetComponentsInChildren<Rigidbody>();

        if (ActivateOnStart) RagdollActive(true);    
        else RagdollActive(false);
    }   

    //Ativa ou desativa os rigidbodies
    public void RagdollActive(bool b) 
    {
        foreach (Rigidbody rb in rbs) rb.isKinematic = !b;

        Debug.Log("<b><color=magenta>" + transform.name + "</color></b> ragdoll = <b><color=cyan>" + b + "</color></b>");
    }
}
