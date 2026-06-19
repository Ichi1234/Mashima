using System;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;

    protected PlayerState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;
    }

    public override void Update()
    {
        base.Update();

        if (player.Input.Player.Crouch.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.CrouchState);
        }
    }
}
