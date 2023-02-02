using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    public static UI_Player instance;

    void Awake() => instance = this;
}
