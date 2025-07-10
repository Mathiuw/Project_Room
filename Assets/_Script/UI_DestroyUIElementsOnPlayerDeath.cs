using UnityEngine;

public class UI_DestroyUIElementsOnPlayerDeath : MonoBehaviour
{

    void Start() 
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player) player.GetComponent<Health>().onDead += OnPlayerDead;
        else Debug.LogError("Cant find Player");
    }

    void OnPlayerDead() 
    {
        Destroy(gameObject);
    }
}
