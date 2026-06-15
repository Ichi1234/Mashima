using UnityEngine;

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

        if (player.Input.Player.Run.IsPressed() && stateMachine.currentState != player.CrouchState)
        {
            player.SetMoveSpeedMultiplier(player.RunSpeedMultiplier);
        }
        else if (stateMachine.currentState != player.CrouchState)
        {
            player.ResetMoveSpeedMultiplier();
        }

        Vector3 moveVertical = player.transform.forward * moveInputWithSpeed.y;
        Vector3 moveHorizontal = player.transform.right * moveInputWithSpeed.x;

        player.MoveCharacter(moveVertical + moveHorizontal);
        
    }
}
