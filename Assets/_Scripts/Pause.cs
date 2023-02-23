using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause instance { get; private set; }

    [SerializeField] Transform pauseTransform;
    [SerializeField] Button resume;
    [SerializeField] Button exit;

    bool isPaused = false;
    public event Action<bool> onPause;

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
        onPause?.Invoke(isPaused);
    }

    void SetPause()
    {
        for (int i = 0; i < pauseTransform.childCount; i++) pauseTransform.GetChild(i).gameObject.SetActive(isPaused);
        if (isPaused)CursorState.CursorUnlock();
        else CursorState.CursorLock();
    }
}
