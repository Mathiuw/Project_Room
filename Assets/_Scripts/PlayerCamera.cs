using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    [SerializeField] Transform gunHolder;
    public Transform GunHolder { get => gunHolder; private set => gunHolder = value; }

    Transform DesiredCameraTransform;

    void Start() 
    {
        if (Pause.instance != null) Pause.instance.onPause += OnPause;
        else Debug.LogError("Cant Find PauseUI");

        //Search player
        Player player = FindObjectOfType<Player>();

        if (player)
        {
            player.GetComponent<Health>().onDead += OnDead;

            //Set the position of the camera
            DesiredCameraTransform = player.CameraPosition;
        }
        else Debug.LogError("Cant find Player");

    }

    void Update()
    {
        transform.position = DesiredCameraTransform.position;
    }

    void OnPause(bool b) 
    {
        GetComponentInChildren<PlayerCameraMove>().enabled= !b;
        GetComponentInChildren<PlayerCameraMove>().enabled= !b;
    }
    
    void OnDead() 
    {
        GetComponentInChildren<PlayerCameraMove>().enabled = false;
        GetComponentInChildren<PlayerCameraMove>().enabled = false;
    }
}
