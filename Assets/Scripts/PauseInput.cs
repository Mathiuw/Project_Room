using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseInput : MonoBehaviour
{
    void Update() 
    {      
        if (Input.GetKeyDown(KeyCode.Escape)) Pause.instance.OnPauseUnpause();
    }
}
