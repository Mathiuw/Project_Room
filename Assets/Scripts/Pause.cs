using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool isPaused;

    public event Action<bool> changePauseState;

    [SerializeField]private GameObject[] uiElements;

    private void Awake()
    {
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
            foreach (GameObject ui in uiElements)
            {
                ui.SetActive(true);
            }
            CursorState.CursorUnlock();
        }
        else
        {
            foreach (GameObject ui in uiElements)
            {
                ui.SetActive(false);
            }
            CursorState.CursorLock();
        }
    }
}
