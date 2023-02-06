using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] RectTransform loadingScreen;
    [SerializeField] RectTransform mainMenu;

    [Header("Buttoms")]
    [SerializeField] Button newGame;
    [SerializeField] Button exit;

    void Awake() 
    {
        CursorState.CursorUnlock();
        mainMenu.gameObject.SetActive(true);
        loadingScreen.gameObject.SetActive(false);
    }

    void Start() 
    {
        newGame.onClick.AddListener(ManagerGame.instance.StartGame);
        newGame.onClick.AddListener(SetLoadScreen);
        exit.onClick.AddListener(ManagerGame.instance.ExitGame); 
    }

    public void SetLoadScreen() 
    {
        if(loadingScreen != null)loadingScreen.gameObject.SetActive(true);
        if (mainMenu != null) Destroy(mainMenu.gameObject);
    }
}
