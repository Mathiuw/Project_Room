using UnityEngine;
using UnityEngine.AI;

public class DEBUG_Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        agent.destination = target.position;
    }
}
