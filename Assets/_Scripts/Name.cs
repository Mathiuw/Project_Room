using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour
{
    [field: SerializeField] public string text { get; private set; }

    public void SetText(string text) 
    {
        this.text = text;
    }
}
