using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    [SerializeField] ElevatorPanel panelInside;

    void Start() => panelInside.onButtomPress += EndGame;

    void EndGame() => StartCoroutine(EndGameCoroutine());

    IEnumerator EndGameCoroutine() 
    {      
        //while (UI_Fade.instance.alpha < 1f) yield return null;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(2);
        Debug.Log("<b><color=green>Finished game</color></b>");
        yield break;
    }
}
