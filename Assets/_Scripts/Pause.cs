using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause instance;

    bool isPaused = false;
    public event Action<bool> Paused;

    void Awake() => instance = this;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OnPauseUnpause();
    }

    void OnPauseUnpause()
    {
        isPaused = !isPaused;
        CheckUIElementsAndCursorState();
        Paused?.Invoke(isPaused);
    }

    void CheckUIElementsAndCursorState()
    {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(isPaused);
        if (isPaused)CursorState.CursorUnlock();
        else CursorState.CursorLock();
    }
}
