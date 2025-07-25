using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] UI_Fade fade;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void RestartLevelTransition() 
    {
        SceneTransition(SceneManager.GetActiveScene().buildIndex);
    }

    public void SceneTransition(int sceneIndex) 
    {
        StartCoroutine(SceneTransitionCoroutine(sceneIndex));
    }

    private IEnumerator SceneTransitionCoroutine(int sceneIndex) 
    {
        UI_Fade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
        fade.FadeIn();

        while (fade.alpha < 1)
        {
            yield return null;
        }

        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);

        yield break;
    }
}
