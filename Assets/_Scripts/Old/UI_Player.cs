using UnityEngine;

public class UI_Player : MonoBehaviour
{

    void Start() 
    {
        Player player = FindAnyObjectByType<Player>();

        if (player) player.GetComponent<Health>().onDead += OnDead;
        else Debug.LogError("Cant Find <color=magenta><b>Player</color></b>");
    }

    void OnDead() 
    {
        Destroy(this);
    }
}
