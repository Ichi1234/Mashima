using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected float moveSpeedMultiplier = 1;

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.CallUpdateCurrentState();
    }

    protected virtual void FixedUpdate() => stateMachine.CallFixedUpdateCurrentState();

    public void ResetMoveSpeedMultiplier() => moveSpeedMultiplier = 1;
    public void SetMoveSpeedMultiplier(float newMultiplier) => moveSpeedMultiplier = newMultiplier;
}
