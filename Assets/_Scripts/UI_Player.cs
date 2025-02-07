using UnityEngine;

public class UI_Player : MonoBehaviour
{

    void Start() 
    {
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();

        if (player) player.GetComponent<Health>().onDead += OnDead;
        else Debug.LogError("Cant find Player");
    }

    void OnDead() 
    {
        Destroy(this);
    }
}
