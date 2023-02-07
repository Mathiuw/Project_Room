using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform position;

    void Update() 
    {
        transform.position = position.position;
    }
}