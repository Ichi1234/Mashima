using UnityEngine;

public class EntityState : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    protected float stateTimer;

    protected EntityState(Entity entity, StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {
    }
}
