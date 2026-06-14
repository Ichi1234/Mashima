using System;
using UnityEngine;

public abstract class PlayerState
{
    public StateMachine stateMachine { get; private set; }

    protected float stateTimer;


    protected PlayerState(Player Player, StateMachine stateMachine)
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
