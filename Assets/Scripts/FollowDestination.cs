using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowDestination : MonoBehaviour
{
    public Transform Destination = null;
    private NavMeshAgent ThisAgent = null;

    private void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        ThisAgent.SetDestination(Destination.position);
    }
}
