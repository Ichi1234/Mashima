using UnityEngine;

public class Pursuer_ChaseState : PursuerState
{
    private float chaseTime = 4.5f;

    public Pursuer_ChaseState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        pursuer.SetMoveSpeedMultiplier(pursuer.ChaseSpeedMultiplier);
        GameManager.Instance.SetAppoximateNoise(0);

        pursuer.Animation.SetRunning(true);

    }

    public override void Update()
    {
        base.Update();

        if (pursuer.IsSeeingPlayer)
        {
            stateTimer = chaseTime;
        }

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(pursuer.LosePlayerState);
        }

        pursuer.LookAtPlayer();
        pursuer.UpdateDestination(GameManager.Instance.PlayerAppoximatedLocation());
    }

    public override void Exit()
    {
        base.Exit();

        GameManager.Instance.ResetAppoximateNoise();
        pursuer.ResetMoveSpeedMultiplier();

        pursuer.Animation.SetRunning(false);
        pursuer.ResetLook();

    }
}
