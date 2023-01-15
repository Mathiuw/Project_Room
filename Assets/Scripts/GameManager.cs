using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject loadingScreen;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;


        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        loadingScreen.SetActive(true);

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
