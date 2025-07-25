using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UI_Fade fade;

    private void Start()
    {
        // Start fade out
        UI_Fade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
        fade.FadeOut();

        // Restart level when player die
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();

        if (playerMovement)
        {
            Health health = playerMovement.GetComponent<Health>();

            if (health && GameManager.Instance)
            {
                health.onDead += GameManager.Instance.RestartLevelTransition;
            }
        }
    }
}
