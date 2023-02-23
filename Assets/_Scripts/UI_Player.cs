using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    public static UI_Player instance;

    void Awake() => instance = this;

    void Start() 
    {
        if (Player.instance != null) Player.instance.GetComponent<Die>().onDead += OnDead;
        else Debug.LogError("Cant Find Player");
    }

    void OnDead() 
    {
        gameObject.SetActive(false);
    }
}
