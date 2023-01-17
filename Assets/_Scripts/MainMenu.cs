using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject loadingScreen;

    void Start() 
    {
        mainMenu.SetActive(true);
        loadingScreen.SetActive(false);      
    }

    public void LoadLevel() 
    {
        Destroy(mainMenu);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void ExitGame() 
    {
        Application.Quit();  
    }
}
