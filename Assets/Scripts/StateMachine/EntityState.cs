using UnityEngine;

public abstract class EntityState
{
    public StateMachine stateMachine { get; private set; }
    protected float stateTimer;
    protected Entity entity;

    protected EntityState(Entity entity, StateMachine stateMachine)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void FixedUpdate() { }

    public virtual void Exit() { }
}
