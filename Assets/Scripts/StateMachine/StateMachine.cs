using UnityEngine;

public class StateMachine
{
    public PlayerState currentState;
    public bool CanChangeState { get; private set; } = true;


    public void Initialize(PlayerState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        if (CanChangeState)
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

    public void lockedState() => CanChangeState = false;
    public void unLockedState() => CanChangeState = true;
}
