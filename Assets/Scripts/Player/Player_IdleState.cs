using UnityEngine;

public class Player_IdleState : PlayerState
{
    public Player_IdleState(Player Player, StateMachine stateMachine) : base(Player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("I'm enter");
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("test");
    }
}
