using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManagent : MonoBehaviour
{
    public GameObject loadingScreen;

    public static int levelIndex = 0;

    [SerializeField] private int levelAmount;

    public List<string> gameScenes = new List<string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        gameManagent[] instances = FindObjectsOfType<gameManagent>();

        if (instances.Length > 1)
        {
            for (int i = 0; i < instances.Length - 1; i++)
            {
                Destroy(instances[i].gameObject);
            }
        }

        for (int i = 1; i < levelAmount+1; i++)
        {
            gameScenes.Add("Level" + i.ToString());
        }
    }

    public void StartGame()
    {
        levelIndex = 0;

        loadingScreen.SetActive(true);

        foreach (string scene in gameScenes)
        {
            if (gameScenes.IndexOf(scene) == levelIndex)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            }
        }
    }

    public IEnumerator NextLevel()
    {
        Scene previousScene = SceneManager.GetActiveScene();

        Scene nextScene;

        levelIndex++;

        foreach (string scene in gameScenes)
        {
            if (gameScenes.IndexOf(scene) == levelIndex)
            {
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                nextScene = SceneManager.GetSceneByName(scene);

                while (!loadScene.isDone)
                {
                    yield return null;
                }

                GameObject[] previousSceneGO = previousScene.GetRootGameObjects();

                foreach (GameObject GO in previousSceneGO)
                {
                    if (GO.GetComponent<Elevator>())
                    {
                        continue;
                    }
                    else
                    {
                        Destroy(GO);
                    }
                }

                SceneManager.SetActiveScene(nextScene);
                yield break;
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
