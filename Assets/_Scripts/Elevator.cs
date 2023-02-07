using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] ElevatorPanel panelToEnd;

    void Start() => panelToEnd.onButtomPress += EndGame;

    void EndGame() => StartCoroutine(End());

    IEnumerator End() 
    {
        UI_Fade.instance.FadeIn();
        
        while (UI_Fade.instance.alpha < 1f) yield return null;

        yield return new WaitForSeconds(1f);

        ManagerGame.instance.LoadLevel(2);
        yield break;
    }
}
