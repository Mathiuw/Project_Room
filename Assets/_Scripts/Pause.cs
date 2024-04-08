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

    void Awake() 
    {
        instance = this;

        AlternateSprites();
        resume.onClick.AddListener(SetPause);
        if (ManagerGame.instance != null) exit.onClick.AddListener(ManagerGame.instance.ExitGame);
    } 

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SetPause();
    }

    void AlternateSprites() 
    {
        menuBackground.gameObject.SetActive(isPaused);
        menuPause.gameObject.SetActive(isPaused);
        menuOptions.gameObject.SetActive(false);
    }

    void SetPause()
    {
        isPaused = !isPaused;

        AlternateSprites();

        if (isPaused) CursorState.CursorUnlock();
        else CursorState.CursorLock();

        onPause?.Invoke(isPaused);
    }
}
