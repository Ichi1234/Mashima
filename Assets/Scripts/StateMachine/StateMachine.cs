using UnityEngine;

public class StateMachine
{
    public PlayerState currentState;
    private bool canChangeState = true;

    public void Initialize(PlayerState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        if (canChangeState)
        {
            currentState.Exit();

            currentState = newState;

            currentState.Enter();
        }
    }

    public void CallUpdateCurrentState()
    {
        currentState.Update();
    }

    public void CallFixedUpdateCurrentState()
    {
        currentState.FixedUpdate();
    }

    public void lockedState() => canChangeState = false;
    public void unLockedState() => canChangeState = true;
}
