using UnityEngine;
using static Player;

public class Player_CrouchState : Player_MoveState
{
    public Player_CrouchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.lockedState();
        player.SetCrouchHitbox();
        player.SetMoveSpeedMultiplier(player.CrouchSpeedMultiplier);

        if (player.PlayerMode == PlayerModes.Desktop)
            player.MoveCamera(new Vector2(0, player.CrouchCameraPosition));
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

        player.SetDefaultHitbox();
        player.ResetMoveSpeedMultiplier();

        if (player.PlayerMode == PlayerModes.Desktop)
            player.ResetCameraPos();
    }
}
