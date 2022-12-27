using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField]
    bool ActivateOnStart = false;

    [SerializeField]
    Rigidbody[] rbs;
   
    void Awake() 
    {
        rbs = GetComponentsInChildren<Rigidbody>();

        if (ActivateOnStart) RagdollActive(true);    
        else RagdollActive(false);
    }   

    public void RagdollActive(bool b) 
    {
        foreach (Rigidbody rb in rbs) rb.isKinematic = !b;
        Debug.Log("Ragdoll = " + b);
    }
}
