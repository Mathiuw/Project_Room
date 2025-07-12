using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField]float time = 5f;

    void Start() => StartCoroutine(ReturnToMenuCoroutine(time));

    IEnumerator ReturnToMenuCoroutine(float time) 
    {
        yield return new WaitForSeconds(time);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
        yield break;
    }
}
