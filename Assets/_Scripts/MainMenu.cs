using UnityEngine;
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
        CursorState.CursorUnlock();
        menu.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
        loadingScreen.gameObject.SetActive(false);

        if (ManagerGame.instance != null)
        {
            newGame.onClick.AddListener(ManagerGame.instance.StartGame);
            exit.onClick.AddListener(ManagerGame.instance.ExitGame);
        }
  
        newGame.onClick.AddListener(SetLoadScreen);
    }

    public void SetLoadScreen() 
    {
        loadingScreen.gameObject.SetActive(true);
        Destroy(menu.gameObject);
    }
}
