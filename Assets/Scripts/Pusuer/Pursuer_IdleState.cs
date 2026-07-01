using UnityEngine;

public class Pursuer_IdleState : PursuerState
{
    private int minWaitingTime = 1;
    private int maxWaitingTime = 3;

    public Pursuer_IdleState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = Random.Range(minWaitingTime, maxWaitingTime);

        pursuer.Animation.SetIdle(true);

    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(pursuer.PatrolState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        pursuer.Animation.SetIdle(false);
    }
}
