using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] private SelectType selectType;

    public enum SelectType
    {
        DestroyOnLoad,
        DontDestroyOnLoad,
    }

    private void Awake()
    {
        switch (selectType)
        {
            case SelectType.DestroyOnLoad:
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
                break;
            case SelectType.DontDestroyOnLoad:
                DontDestroyOnLoad(gameObject);
                break;
        }
    }
}