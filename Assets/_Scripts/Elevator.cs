using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] int changeSceneID;
    [SerializeField] ElevatorPanel panelToEnd;

    void Start() => panelToEnd.onButtomPress += EndGame;

    void EndGame() => StartCoroutine(ChangeScene());

    IEnumerator ChangeScene() 
    {
        UI_Fade.instance.FadeIn();
        
        while (UI_Fade.instance.alpha < 1f) yield return null;

        yield return new WaitForSeconds(1f);

        ManagerGame.instance.LoadLevel(changeSceneID);
        Debug.Log("<b><color=green>Finished game</color></b>");
        yield break;
    }
}
