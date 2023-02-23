using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform position;

    void Start() 
    {
        if (Player.instance != null) position = Player.instance.CameraPosition;
        else Debug.LogError("Cant Find Player");
    }

    void Update() => transform.position = position.position;
}