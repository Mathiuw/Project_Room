using UnityEngine;

public class UI_Player : MonoBehaviour
{
    public static UI_Player instance;

    void Awake() => instance = this;

    void Start() 
    {
        if (Player.instance != null) Player.instance.GetComponent<Health>().onDead += OnDead;
        else Debug.LogError("Cant Find <color=magenta><b>Player</color></b>");
    }

    void OnDead() 
    {
        gameObject.SetActive(false);
    }
}
