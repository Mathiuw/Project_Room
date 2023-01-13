using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagent : MonoBehaviour
{
    public GameObject loadingScreen;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameManagent[] instances = FindObjectsOfType<GameManagent>();

        if (instances.Length > 1)
        {
            for (int i = 0; i < instances.Length - 1; i++)
            {
                Destroy(instances[i].gameObject);
            }
        }
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
