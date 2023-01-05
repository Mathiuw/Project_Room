using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] bool ActivateOnStart = false;
    [SerializeField] Rigidbody[] rbs;
    Enemy enemy;
   
    void Start() 
    {
        rbs = GetComponentsInChildren<Rigidbody>();

        if (ActivateOnStart) RagdollActive(true);    
        else RagdollActive(false);
    }   

    //Ativa ou Desativa o Ragdoll
    public void RagdollActive(bool b) 
    {
        foreach (Rigidbody rb in rbs) rb.isKinematic = !b;
        Debug.Log("Ragdoll = " + b);
    }

    //Ativa o Ragdoll
    public void OnActivateRagdoll() => RagdollActive(true);
}
