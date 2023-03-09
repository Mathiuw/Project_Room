using System;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public static Pause instance { get; private set; }

    [SerializeField] Transform menuBackground;
    [SerializeField] Transform menuPause;
    [SerializeField] Transform menuOptions;
    [SerializeField] Button resume;
    [SerializeField] Button exit;

    bool isPaused = false;
    public event Action<bool> onPause;

    void Awake() => instance = this;

    void Start() 
    {
        SetSprites();
        resume.onClick.AddListener(SetPause);
        if (ManagerGame.instance != null) exit.onClick.AddListener(ManagerGame.instance.ExitGame);
        else Debug.LogError("Cant Find Game Manager");
    } 

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SetPause();
    }

    void SetSprites() 
    {
        if (isPaused) 
        {
            menuBackground.gameObject.SetActive(true);
            menuPause.gameObject.SetActive(true);
            menuOptions.gameObject.SetActive(false);
        }
        else
        {
            menuBackground.gameObject.SetActive(false);
            menuPause.gameObject.SetActive(false);
            menuOptions.gameObject.SetActive(false);
        }
    }

    void SetPause()
    {
        isPaused = !isPaused;

        SetSprites();

        if (isPaused) CursorState.CursorUnlock();
        else CursorState.CursorLock();

        onPause?.Invoke(isPaused);
    }
}
