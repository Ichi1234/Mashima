using System;
using UnityEngine;
using UnityEngine.AI;

public class Pursuer : Entity
{
    [SerializeField] private float moveSpeed = 4.6f;
    [SerializeField] private float chaseSpeedMultiplier = 1.25f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;

    [SerializeField] private float playerDetectionRange;
    [SerializeField] private Transform pursuerEyes;
    [SerializeField] private float fovAngle = 90;
    [SerializeField] private float horizontalAngle = 90f;
    [SerializeField] private float verticalAngle = 60f;

    [SerializeField] private NavMeshAgent agent;

    public Action OnReachedTheDesitnation;

    public Pursuer_IdleState IdleState { get; private set; }
    public Pursuer_PatrolState PatrolState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        agent.speed = moveSpeed;
       
        IdleState = new Pursuer_IdleState(this, stateMachine);
        PatrolState = new Pursuer_PatrolState(this, stateMachine);

        stateMachine.Initialize(PatrolState);

    }

    protected override void Update()
    {
        base.Update();

        agent.speed = moveSpeed * moveSpeedMultiplier;

        if (!agent.pathPending && agent.remainingDistance <= 0.02f)
        {
            OnReachedTheDesitnation?.Invoke();
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float rayRange = playerDetectionRange;
        float halfHorizontalFOV = horizontalAngle / 2.0f;
        float halfVerticalFOV = verticalAngle / 2.0f;

        Vector3 forward = pursuerEyes.forward;

        Vector3 topLeft = Quaternion.Euler(-halfVerticalFOV, -halfHorizontalFOV, 0) * forward;
        Vector3 topRight = Quaternion.Euler(-halfVerticalFOV, halfHorizontalFOV, 0) * forward;
        Vector3 bottomLeft = Quaternion.Euler(halfVerticalFOV, -halfHorizontalFOV, 0) * forward;
        Vector3 bottomRight = Quaternion.Euler(halfVerticalFOV, halfHorizontalFOV, 0) * forward;

        Vector3 origin = pursuerEyes.position;

        Gizmos.DrawRay(origin, topLeft * rayRange);
        Gizmos.DrawRay(origin, topRight * rayRange);
        Gizmos.DrawRay(origin, bottomLeft * rayRange);
        Gizmos.DrawRay(origin, bottomRight * rayRange);

        Gizmos.DrawLine(origin + topLeft * rayRange, origin + topRight * rayRange);
        Gizmos.DrawLine(origin + topRight * rayRange, origin + bottomRight * rayRange);
        Gizmos.DrawLine(origin + bottomRight * rayRange, origin + bottomLeft * rayRange);
        Gizmos.DrawLine(origin + bottomLeft * rayRange, origin + topLeft * rayRange);
    }


    public void UpdateDestination(Vector3 newDestination) => agent.destination = newDestination;
}
