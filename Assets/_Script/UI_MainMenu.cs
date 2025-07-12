using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] RectTransform menu;
    [SerializeField] RectTransform options;
    [SerializeField] RectTransform loadingScreen;

    [Header("Buttoms")]
    [SerializeField] Button newGame;
    [SerializeField] Button exit;

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        menu.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(false);

        newGame.onClick.AddListener(StartGame);
        newGame.onClick.AddListener(SetLoadScreen);

        exit.onClick.AddListener(Application.Quit);
    }

    private void StartGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void SetLoadScreen() 
    {
        loadingScreen.gameObject.SetActive(true);
        Destroy(menu.gameObject);
    }
}
