using UnityEngine;

public class Pursuer_RoarState : PursuerState
{
    public Pursuer_RoarState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        pursuer.Animation.PlayRoar();

        pursuer.StopMovement();

        pursuer.Animation.OnAnimationFinished += OnRoarFinished;
    }

    public override void Exit()
    {
        base.Exit();

        pursuer.Animation.OnAnimationFinished -= OnRoarFinished;
    }

    public void OnRoarFinished()
    {
        pursuer.ResumeMovement();

        stateMachine.ChangeState(pursuer.IdleState);
    }
}
