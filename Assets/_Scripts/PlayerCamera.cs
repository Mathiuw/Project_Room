using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance { get; private set; }
    
    [SerializeField] Transform gunHolder;
    public Transform GunHolder { get => gunHolder; private set => gunHolder = value; }

    void Awake() => instance = this;

    void Start() 
    {
        if (Pause.instance != null) Pause.instance.onPause += OnPause;
        else Debug.LogError("Cant Find Player UI");
        Player.instance.GetComponent<Health>().onDead += OnDead; 
    }

    void OnPause(bool b) 
    {
        GetComponentInChildren<PlayerCameraMove>().enabled= !b;
        GetComponentInChildren<CameraRotateSideways>().enabled= !b;
        GetComponentInChildren<PlayerCameraMove>().enabled= !b;
    }
    
    void OnDead() 
    {
        GetComponentInChildren<PlayerCameraMove>().enabled = false;
        GetComponentInChildren<CameraRotateSideways>().enabled = false;
        GetComponentInChildren<PlayerCameraMove>().enabled = false;
    }
}
