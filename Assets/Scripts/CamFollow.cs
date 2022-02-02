using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform camPlace;

    void Update()
    {
        transform.position = camPlace.transform.position;
    }
}