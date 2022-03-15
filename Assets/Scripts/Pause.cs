using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool isPaused;

    public event Action<bool> changePauseState;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        isPaused = false;

        CheckUIElementsAndCursorState(isPaused);

        changePauseState += CheckUIElementsAndCursorState;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrUnpauseGame();
        }
    }

    public void PauseOrUnpauseGame()
    {
        isPaused = !isPaused;

        changePauseState?.Invoke(isPaused);
    }

    public void CheckUIElementsAndCursorState(bool paused)
    {
        if (paused)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            CursorState.CursorUnlock();
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            CursorState.CursorLock();
        }    
    }
}
