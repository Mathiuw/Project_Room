﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EFadeType
{
    FadeIn, FadeOut
}

public class UI_Fade : MonoBehaviour
{
    [SerializeField] float fadeTime = 1f;
    [SerializeField] AnimationCurve curve;
    Image image;

    public float alpha { get; private set; }

    void Awake() 
    {
        image= GetComponentInChildren<Image>();
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

    private IEnumerator FadeCoroutine(float initial,float final) 
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