using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Fade : MonoBehaviour
{
    public enum EFadeType 
    {
        FadeIn, FadeOut
    }

    [SerializeField] float fadeTime = 1f;
    [SerializeField] AnimationCurve curve;
    [field: SerializeField] public EFadeType fadeType { get; set; } = EFadeType.FadeOut;
    Image image;

    public float alpha { get; private set; }

    void Awake() 
    {
        image= GetComponentInChildren<Image>();
    }

    IEnumerator Start() 
    {
        SetImageAlphaValue(1f);
        yield return new WaitForSeconds(0.1f);
        FadeOut();
        yield break;
    } 

    public void SetImageAlphaValue(float value) 
    {
        Color color = image.color;
        color.a = value;
        
        alpha = color.a;
        image.color = color;
    }

    public void FadeIn() => StartCoroutine(FadeCoroutine(0, 1));

    public void FadeOut() => StartCoroutine(FadeCoroutine(1, 0));

    public IEnumerator FadeCoroutine(float initial,float final) 
    {
        float timePassed = 0;

        SetImageAlphaValue(initial);
        while (timePassed < fadeTime)
        {
            SetImageAlphaValue(curve.Evaluate(Mathf.Lerp(initial, final, timePassed)));
            timePassed += (Time.deltaTime / fadeTime);

            yield return null;
        }
        SetImageAlphaValue(final);

        yield break;
    }
}