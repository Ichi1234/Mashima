using UnityEngine;

public class StateMachine
{
    public EntityState currentState;
    public bool CanChangeState { get; private set; } = true;


    public void Initialize(EntityState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)
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
