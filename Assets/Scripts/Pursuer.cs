using UnityEngine;
using UnityEngine.AI;

public class Pursuer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.6f;
    [SerializeField] private float chaseSpeedMultiplier = 1.25f;


    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;

    private void Update()
    {
        agent.destination = player.position;
    }
}
