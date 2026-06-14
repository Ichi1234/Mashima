using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController charController;

    private StateMachine stateMachine;

    private Player_IdleState idleState;

    private void Awake()
    {
        stateMachine = new StateMachine();

        idleState = new Player_IdleState(this, stateMachine);

        stateMachine.Initialize(idleState);

    }

    private void Update() => stateMachine.CallUpdateCurrentState();

    private void FixedUpdate() => stateMachine.CallFixedUpdateCurrentState();
}
