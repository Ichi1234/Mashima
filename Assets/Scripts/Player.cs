using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [Header("Play Mode")]
    [SerializeField] private PlayerModes playerMode;
    public enum PlayerModes { Desktop, VR }
    [Space]

    [Header("General Details")]
    [SerializeField] private CharacterController charController;
    [SerializeField] private float gravity = 0.98f;
    [SerializeField] private Transform cameraOffset;

    [Header("Interact Details")]
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayer;
    public bool isInteractabled { get; private set; }

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
    public PlayerInputSet Input { get; private set; }
    public Vector2 MoveInput { get; private set; }

    private float moveSpeedMultiplier = 1;
    private StateMachine stateMachine;

    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }
    public Player_CrouchState CrouchState { get; private set; }

    public float MoveSpeed => moveSpeed;
    public float RunSpeedMultiplier => runSpeedMultiplier;
    public float CrouchSpeedMultiplier => crouchSpeedMultiplier;
    public float CrouchCameraPosition => crouchCameraPosition;
    public PlayerModes PlayerMode => playerMode;


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
        RaycastHit hit = CameraInteractRaycast();

        if (Input.Player.Interact.WasPerformedThisFrame() && isInteractabled)
        {
            hit.transform.GetComponent<Item>().Interact();
        }

        ApplyGravity();


        stateMachine.CallUpdateCurrentState();
    }

    private void ApplyGravity()
    {
        charController.Move(Vector3.down * gravity * Time.deltaTime);
    }

    private RaycastHit CameraInteractRaycast()
    {
        isInteractabled = Physics.Raycast
        (
            cameraOffset.transform.position,
            cameraOffset.transform.forward,
            out RaycastHit hit,
            interactDistance,
            interactLayer
         );

        return hit;
    }

    private void FixedUpdate() => stateMachine.CallFixedUpdateCurrentState();

    private void OnDisable()
    {
        Input.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(
            cameraOffset.transform.position,
            cameraOffset.transform.position + cameraOffset.transform.forward * interactDistance
        );
    }

    public void ResetMoveSpeedMultiplier() => moveSpeedMultiplier = 1;

    public void SetMoveSpeedMultiplier(float newMultiplier) => moveSpeedMultiplier = newMultiplier;

    public void MoveCharacter(Vector3 moveDir) => charController.Move(moveDir * moveSpeedMultiplier * Time.deltaTime);

    public void MoveCamera(Vector2 newPosition) => cameraOffset.localPosition = new Vector3(0, newPosition.y, 0);

    public void ResetCameraPos() => cameraOffset.localPosition = Vector3.zero;

    public void RotateCamera(Quaternion newAngle) => cameraOffset.transform.localRotation = newAngle;

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

    public Vector3 HMDForwardFlat()
    {
        Vector3 forward = cameraOffset.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 HMDRightFlat()
    {
        Vector3 right = cameraOffset.transform.right;
        right.y = 0;
        return right.normalized;
    }
}
