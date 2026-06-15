using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController charController;
    [SerializeField] private float gravity = 0.98f;

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 4.4f;
    [SerializeField] private float runSpeedMultiplier = 1.25f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float crouchCameraPosition = -0.82f;
    [SerializeField] private float crouchHitboxRadius = 0.2f;
    [SerializeField] private float crouchHitboxHeight = 0.1f;
    [SerializeField] private float crouchHitboxCenter = -0.72f;
    [SerializeField] private float defaultHitboxRadius = 0.5f;
    [SerializeField] private float defaultHitboxHeight = 2;
    [SerializeField] private Camera playerCamera;

    private float moveSpeedMultiplier = 1;
    private StateMachine stateMachine;

    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }
    public Player_CrouchState CrouchState { get; private set; }

    public PlayerInputSet Input { get; private set; }

    public Vector2 MoveInput { get; private set; }

    public float MoveSpeed => moveSpeed;
    public float RunSpeedMultiplier => runSpeedMultiplier;
    public float CrouchSpeedMultiplier => crouchSpeedMultiplier;
    public float CrouchCameraPosition => crouchCameraPosition;


    private void Awake()
    {
        Input = new PlayerInputSet();

        stateMachine = new StateMachine();

        IdleState = new Player_IdleState(this, stateMachine);
        MoveState = new Player_MoveState(this, stateMachine);
        CrouchState = new Player_CrouchState(this, stateMachine);

        stateMachine.Initialize(IdleState);

    }

    private void OnEnable()
    {
        Input.Enable();

        Input.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        Input.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void Update()
    {
        charController.Move(Vector3.down * gravity * Time.deltaTime);
        stateMachine.CallUpdateCurrentState();
    }

    private void FixedUpdate() => stateMachine.CallFixedUpdateCurrentState();

    private void OnDisable()
    {
        Input.Disable();
    }

    public void ResetMoveSpeedMultiplier() => moveSpeedMultiplier = 1;

    public void SetMoveSpeedMultiplier(float newMultiplier) => moveSpeedMultiplier = newMultiplier;

    public void MoveCharacter(Vector3 moveDir) => charController.Move(moveDir * moveSpeedMultiplier * Time.deltaTime);

    public void MoveCamera(Vector2 newPosition) => playerCamera.transform.localPosition = newPosition;
   
    public void ResetCameraPos() => playerCamera.transform.localPosition = new Vector2(0, 0);

    public void RotateCamera(Quaternion newAngle) => playerCamera.transform.localRotation = newAngle;

    public void SetCrouchHitbox()
    {
        charController.height = crouchHitboxHeight;
        charController.radius = crouchHitboxRadius;
        charController.center = new Vector3(0, crouchHitboxCenter, 0);
    }

    public void SetDefaultHitbox()
    {
        charController.height = defaultHitboxHeight;
        charController.radius = defaultHitboxRadius;
        charController.center = new Vector3(0, 0, 0);
    }
}
