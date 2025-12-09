using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    public Transform counterPoint;  // <-- MUST BE Transform

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(counterPoint.position);
    }
}
