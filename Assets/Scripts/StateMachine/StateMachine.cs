using UnityEngine;

public class StateMachine
{
    public PlayerState currentState;

    public void Initialize(PlayerState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();

        currentState = newState;

        currentState.Enter();

    }

    public void CallUpdateCurrentState()
    {
        currentState.Update();
    }

    public void CallFixedUpdateCurrentState()
    {
        currentState.FixedUpdate();
    }
}
