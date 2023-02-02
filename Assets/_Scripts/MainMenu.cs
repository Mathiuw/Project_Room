using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadingScreen;

    [Header("Buttoms")]
    [SerializeField] Button newGame;
    [SerializeField] Button exit;

    void Start() 
    {
        mainMenu.SetActive(true);
        loadingScreen.SetActive(false);

        ManagerGame.instance.onNewGame += SetLoadScreen;

        newGame.onClick.AddListener(ManagerGame.instance.StartGame);
        exit.onClick.AddListener(ManagerGame.instance.ExitGame);
    }

    public void SetLoadScreen() 
    {
        loadingScreen.SetActive(true);
        Destroy(mainMenu);
    }
}
