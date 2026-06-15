using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController charController;
    [SerializeField] private float moveSpeed = 4.4f;
    [SerializeField] private float runSpeedMultiplier = 1.25f;

    private StateMachine stateMachine;

    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }

    public PlayerInputSet Input { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public float MoveSpeed => moveSpeed;
    public float RunSpeedMultiplier => runSpeedMultiplier;


    private void Awake()
    {
        Input = new PlayerInputSet();

        stateMachine = new StateMachine();

        IdleState = new Player_IdleState(this, stateMachine);
        MoveState = new Player_MoveState(this, stateMachine);

        stateMachine.Initialize(IdleState);

    }

    private void OnEnable()
    {
        Input.Enable();

        Input.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        Input.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void Update() => stateMachine.CallUpdateCurrentState();

    private void FixedUpdate() => stateMachine.CallFixedUpdateCurrentState();

    private void OnDisable()
    {
        Input.Disable();
    }

    public void MoveCharacter(Vector3 moveDir)
    {
        charController.Move(moveDir * Time.deltaTime);
    }
}
