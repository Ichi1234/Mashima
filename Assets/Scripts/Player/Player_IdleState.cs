using UnityEngine;

public class Player_IdleState : PlayerState
{
    public Player_IdleState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}
