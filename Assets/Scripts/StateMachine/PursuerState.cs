using UnityEngine;

public class PursuerState : EntityState
{
    protected Pursuer pursuer;

    public PursuerState(Pursuer pursuer, StateMachine stateMachine) : base(pursuer, stateMachine)
    {
        this.pursuer = pursuer;
    }
}
