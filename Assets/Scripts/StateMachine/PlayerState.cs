using System;
using UnityEngine;

public abstract class PlayerState
{
    public StateMachine stateMachine { get; private set; }

    protected float stateTimer;

    protected Player player;

    protected PlayerState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
    
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        if (player.Input.Player.Crouch.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.CrouchState);
        }

    }

    public virtual void FixedUpdate() 
    {
    
    }

    public virtual void Exit()
    {
    }
}
