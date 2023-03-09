using UnityEngine;

public class SetChildOnStart : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Transform child;

    void Start() 
    {
        if (child != null) child = transform;

        child.SetParent(parent);
    } 
}
