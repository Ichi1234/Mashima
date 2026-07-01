using UnityEngine;

public class Pursuer_PatrolState : PursuerState
{
    public Pursuer_PatrolState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        pursuer.OnReachedTheDesitnation += LookingAround;
        pursuer.UpdateDestination(GameManager.Instance.PlayerAppoximatedLocation());

        pursuer.Animation.SetWalking(true);
    }

    public override void Exit()
    {
        base.Exit();

        pursuer.OnReachedTheDesitnation -= LookingAround;

        pursuer.Animation.SetWalking(false);
    }

    private void LookingAround() => stateMachine.ChangeState(pursuer.IdleState);

}
