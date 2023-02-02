using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame instance { get; private set; }

    public event Action onGameEnd;
    public event Action onNewGame;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartGame() 
    {
        onNewGame?.Invoke();  
        LoadLevel(1);
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void RestartCurrentLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame() 
    {  
        onGameEnd?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
