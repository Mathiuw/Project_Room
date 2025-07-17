using UnityEngine;

public class UI_DestroyUIOnPlayerDeath : MonoBehaviour
{

    void Start() 
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player) 
        {
            Health health;

            player.TryGetComponent(out health);

            if (health)
            {
                health.onDead += OnPlayerDead;
            }
            else Debug.LogError("Cant find player health");
        } 
        else Debug.LogError("Cant find Player");
    }

    void OnPlayerDead() 
    {
        Destroy(gameObject);
    }
}
