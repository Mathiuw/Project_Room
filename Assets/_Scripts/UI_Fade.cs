using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fade : MonoBehaviour
{
    public static UI_Fade instance { get; private set; }

    Image image;
    [SerializeField] float time;
    [SerializeField] AnimationCurve curve;

    public float alpha { get; private set; }

    void Awake() 
    {
        instance = this;
        image= GetComponentInChildren<Image>();
    }

    IEnumerator Start() 
    {
        SetValue(1f);
        yield return new WaitForSeconds(0.5f);
        FadeOut();
        if (Player.instance != null) Player.instance.GetComponentInChildren<Health>().onDead += RestartLevelFadeIn;
        yield break;
    } 

    public void SetValue(float value) 
    {
        Color color = image.color;
        color.a = value;
        
        alpha = color.a;
        image.color = color;
    }

    public IEnumerator FadeValue(float initial,float final) 
    {
        float timePassed = 0;

        SetValue(initial);
        while (timePassed < time)
        {
            SetValue(curve.Evaluate(Mathf.Lerp(initial, final, timePassed)));
            timePassed += (Time.deltaTime / time);

            yield return null;
        }
        SetValue(final);

        yield break;
    }

    public void FadeIn() => StartCoroutine(FadeValue(0, 1));

    public void FadeOut() => StartCoroutine(FadeValue(1, 0));

    public void RestartLevelFadeIn()
    {
        FadeIn();
        StartCoroutine(WhenRestartLevel());
    }

    IEnumerator WhenRestartLevel() 
    {
        if (ManagerGame.instance == null) 
        {
            Debug.LogError("There is no Game Manager");
            yield break;        
        }

        while (alpha < 1f) yield return null;

        ManagerGame.instance.RestartCurrentLevel();
        yield break;
    }
}
