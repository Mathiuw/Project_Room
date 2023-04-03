using UnityEngine;

public class LevelKeycardLocation : MonoBehaviour
{
    [SerializeField] Transform[] keycards;
    [SerializeField] Transform[] Locations;

    void Start()
    {
        for (int i = 0; i < keycards.Length; i++) 
        {
            keycards[i].position = Locations[i].position;  
        }
        Destroy(this);
    }
}
