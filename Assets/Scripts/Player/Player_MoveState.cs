using UnityEngine;
using static Player;

public class Player_MoveState : PlayerState
{
    public Player_MoveState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        Vector2 moveInputWithSpeed = player.MoveInput * player.MoveSpeed;

        if (player.Input.Player.Run.IsPressed() && stateMachine.CanChangeState)
        {
            player.SetMoveSpeedMultiplier(player.RunSpeedMultiplier);
            player.SetPlayerPushForce(GameManager.Instance.DoorSlamForce);
        }
        else if (stateMachine.currentState != player.CrouchState)
        {
            player.ResetMoveSpeedMultiplier();
            player.ResetPlayerPushForce();
        }

        Vector3 forward = player.CurPlayerMode == PlayerMode.VR
            ? player.HMDForwardFlat()
            : player.transform.forward;

        Vector3 right = player.CurPlayerMode == PlayerMode.VR
            ? player.HMDRightFlat()
            : player.transform.right;

        Vector3 moveVertical = forward * moveInputWithSpeed.y;
        Vector3 moveHorizontal = right * moveInputWithSpeed.x;

        player.MoveCharacter(moveVertical + moveHorizontal);
        
    }
}
