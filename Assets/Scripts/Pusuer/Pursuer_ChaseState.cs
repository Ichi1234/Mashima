using UnityEngine;

public class Pursuer_ChaseState : PursuerState
{
    public Pursuer_ChaseState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        pursuer.SetMoveSpeedMultiplier(pursuer.ChaseSpeedMultiplier);
    }

    public override void Exit()
    {
        base.Exit();

        pursuer.ResetMoveSpeedMultiplier();
    }
}
