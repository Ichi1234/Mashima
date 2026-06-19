using System;
using UnityEngine;
using UnityEngine.AI;

public class Pursuer : Entity
{
    [SerializeField] private float moveSpeed = 4.6f;
    [SerializeField] private float chaseSpeedMultiplier = 1.25f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;

    [SerializeField] private NavMeshAgent agent;

    public Action OnReachedTheDesitnation;

    public Pursuer_IdleState IdleState { get; private set; }
    public Pursuer_PatrolState PatrolState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new Pursuer_IdleState(this, stateMachine);
        PatrolState = new Pursuer_PatrolState(this, stateMachine);

        stateMachine.Initialize(PatrolState);
    }

    protected override void Update()
    {
        base.Update();

        if (!agent.pathPending && agent.remainingDistance <= 0.02f)
        {
            OnReachedTheDesitnation?.Invoke();
        }
       
    }

    public void UpdateDestination(Vector3 newDestination) => agent.destination = newDestination;
}
