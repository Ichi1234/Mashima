using UnityEngine;

public class Player_CrouchState : Player_MoveState
{
    public Player_CrouchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.lockedState();

        player.MoveCamera(new Vector2(0, player.CrouchCameraPosition));
        player.SetMoveSpeedMultiplier(player.CrouchSpeedMultiplier);
    }

    public override void Update()
    {
        base.Update();

        if (player.Input.Player.Crouch.WasPerformedThisFrame())
        {
            stateMachine.unLockedState();
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.ResetMoveSpeedMultiplier();
        player.ResetCameraPos();
    }
}
