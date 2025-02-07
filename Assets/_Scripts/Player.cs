using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform cameraPivot;

    public Transform GetCameraPivot() { return cameraPivot; }

}