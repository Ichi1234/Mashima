using UnityEngine;


public class Player_MoveState : PlayerState
{
    private float walkInterval = 0.5f;
    private float runInterval = 0.3f;
    private float crouchInterval = 1;
    private float footStepTimer = 0;
    private bool wasRunning = false;

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

        bool isRunning = player.Input.Player.Run.IsPressed() && stateMachine.CanChangeState;
        bool isCrouching = stateMachine.currentState == player.CrouchState && stateMachine.CanChangeState;
        ChangeFovToRunning(isRunning);

        Vector2 moveInputWithSpeed = player.MoveInput * player.MoveSpeed;

        if (isRunning)
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

        if (player.Input.Player.Move.IsPressed())
        {
            PlayFootStepSound(isRunning, isCrouching);
        }

    }

    private void ChangeFovToRunning(bool isRunning)
    {
        if (GameManager.Instance.IsInVR)
        {
            return;
        }

        if (isRunning != wasRunning)
        {
            if (isRunning)
                player.SetFOV(player.DefaultFov + 10);
            else
                player.ResetFOV();

            wasRunning = isRunning;
        }
    }

    private void PlayFootStepSound(bool isRunning, bool isCrouching)
    {
        if (footStepTimer <= 0)
        {
            player.PlayFootStepSound();

            footStepTimer = isRunning ? runInterval : walkInterval;

            if (isRunning)
            {
                footStepTimer = runInterval;
            }

            else if (isCrouching)
            {
                footStepTimer = crouchInterval;
            }

            else
            {
                footStepTimer = walkInterval;
            }
        }

        footStepTimer -= Time.deltaTime;
    }

}
