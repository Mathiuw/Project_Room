using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    [SerializeField] Button resume;
    [SerializeField] Button exit;

    bool isPaused = false;
    public event Action<bool> Paused;

    void Awake() => instance = this;

    void Start() 
    {
        SetPause();
        resume.onClick.AddListener(OnPauseUnpause);
        if (ManagerGame.instance != null) exit.onClick.AddListener(ManagerGame.instance.ExitGame);
        else Debug.LogError("Cant Find Game Manager");
    } 

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OnPauseUnpause();
    }

    void OnPauseUnpause()
    {
        isPaused = !isPaused;
        SetPause();
        Paused?.Invoke(isPaused);
    }

    void SetPause()
    {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(isPaused);
        if (isPaused)CursorState.CursorUnlock();
        else CursorState.CursorLock();
    }
}
