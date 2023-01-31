using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class ElevatorAnimations : MonoBehaviour
{
    Animator animator;

    void Awake() => animator = GetComponent<Animator>();

    void Start() 
    {
        SetState(false);

        ElevatorInteraction[] interactions = GetComponentsInChildren<ElevatorInteraction>();

        foreach (ElevatorInteraction interaction in interactions)
        {
            interaction.onButtomPress += InvertState;
        }
    }

    void SetState(bool b) => animator.SetBool("open", b);

    void InvertState() 
    {
        bool state = animator.GetBool("open");

        animator.SetBool("open", !state);
    }
}