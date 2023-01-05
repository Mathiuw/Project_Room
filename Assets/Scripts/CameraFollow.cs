using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform camPlace;

    void Update() 
    {
        transform.position = camPlace.position;
    }
}