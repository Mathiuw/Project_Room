using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField]float time = 5f;

    void Start() => StartCoroutine(Return(time));

    IEnumerator Return(float time) 
    {
        yield return new WaitForSeconds(time);

        UI_Fade.instance.FadeIn();

        while (UI_Fade.instance.alpha < 1f) yield return null; 

        yield return new WaitForSeconds(1f);

        ManagerGame.instance.LoadLevel(0);
        yield break;
    }
}
