using UnityEngine;

public class Pursuer_losePlayerState : PursuerState
{

    private float curStateTime = 10;
    public Pursuer_losePlayerState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        float I_KNOW_YOU_ARE_NERE_HAHAHAHA = GameManager.Instance.DefaultNoise / 2;

        GameManager.Instance.SetAppoximateNoise(I_KNOW_YOU_ARE_NERE_HAHAHAHA);
        
        stateTimer = curStateTime;

        pursuer.Animation.SetRunning(true);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(pursuer.RoarState);
        }

        pursuer.UpdateDestination(GameManager.Instance.PlayerAppoximatedLocation());

    }

        public override void Exit()
        {
            base.Exit();

            GameManager.Instance.ResetAppoximateNoise();

            pursuer.Animation.SetRunning(false);

        }
}
