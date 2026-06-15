using UnityEngine;

public class Player_MoveState : PlayerState
{
    public Player_MoveState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Update()
    {
        base.Update();

        Vector2 moveInputWithSpeed = player.MoveInput * player.MoveSpeed;

        if (player.Input.Player.Run.IsPressed())
        {
            moveInputWithSpeed *= player.RunSpeedMultiplier;
        }

        player.MoveCharacter(new Vector3(moveInputWithSpeed.x, 0, moveInputWithSpeed.y));
        
    }
}
