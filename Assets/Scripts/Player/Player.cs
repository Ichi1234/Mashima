using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.XR;

public class Player : Entity
{
    [Header("Play Mode")]
    [SerializeField] private PlayerMode playerMode;
    [Space]

    [Header("General Details")]
    [SerializeField] private Light flashLight;
    [SerializeField] private CharacterController charController;
    [SerializeField] private float gravity = 0.98f;
    [SerializeField] private float defaultPlayerPushForce = 10;
    private float playerPushForce;

    [Header("Camera")]
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float fovChangeDuration = 2;
    private Coroutine fovCoroutine;
    public float DefaultFov { get; private set; }

    [Header("Interact Details")]
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private float sphereRadius = 0.3f;
    public bool isInteractabled { get; private set; }
    private Indicator curIndicator;

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 4.4f;
    [SerializeField] private float runSpeedMultiplier = 1.25f;
    
    [Header("Crouch Details")]
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float crouchCameraPosition = -0.82f;
    [SerializeField] private float crouchHitboxRadius = 0.2f;
    [SerializeField] private float crouchHitboxHeight = 0.1f;
    [SerializeField] private float crouchHitboxCenter = -0.72f;
    [SerializeField] private float defaultHitboxRadius = 0.5f;
    [SerializeField] private float defaultHitboxHeight = 2;
    [Space]
    // For Enemy Detection
    [SerializeField] private CapsuleCollider detectionCollider;
    [SerializeField] private float crouchDetectionHitboxHeight = 1.5f;
    [SerializeField] private float crouchDetectionHitboxPos = -0.33f;

    private Vector3 initialCameraPos;

    public PlayerInputSet Input { get; private set; }
    public Vector2 MoveInput { get; private set; }

    public Player_IdleState IdleState { get; private set; }
    public Player_MoveState MoveState { get; private set; }
    public Player_CrouchState CrouchState { get; private set; }

    public float MoveSpeed => moveSpeed;
    public float RunSpeedMultiplier => runSpeedMultiplier;
    public CapsuleCollider DetectionCollider => detectionCollider;
    public float CrouchSpeedMultiplier => crouchSpeedMultiplier;
    public float CrouchCameraPosition => crouchCameraPosition;
    public PlayerMode CurPlayerMode => playerMode;


    protected override void Awake()
    {
        base.Awake();

        Input = new PlayerInputSet();

        IdleState = new Player_IdleState(this, stateMachine);
        MoveState = new Player_MoveState(this, stateMachine);
        CrouchState = new Player_CrouchState(this, stateMachine);

        stateMachine.Initialize(IdleState);

        GameManager.Instance.InitializePlayer(this);
        DialogManager.Instance.InitializePlayer(this);

        initialCameraPos = cameraOffset.localPosition;

        playerPushForce = defaultPlayerPushForce;

        DefaultFov = playerCamera.fieldOfView;

    }

    private void OnEnable()
    {
        Input.Enable();

        Input.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        Input.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
    }

    protected override void Update()
    {
        RaycastHit hit = CameraInteractRaycast();

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }

        PlayerInteraction(hit);

        ApplyGravity();

        base.Update();
    }

    private void PlayerInteraction(RaycastHit hit)
    {
        if (!isInteractabled)
        {
            curIndicator?.SetInteractable(false);
        }

        if (Input.Player.NPCInteraction.WasPerformedThisFrame() && isInteractabled)
        {
            hit.transform.GetComponent<INPCInteractable>()?.Interact();
        }

        if (Input.Player.Interact.WasPerformedThisFrame() && isInteractabled)
        {
            hit.transform.GetComponent<IInteractable>()?.Interact();
        }

        if (Input.Player.Flashlight.WasPerformedThisFrame())
        {
            flashLight.enabled = !flashLight.enabled;
        }
    }

    private void ApplyGravity()
    {
        charController.Move(Vector3.down * gravity * Time.deltaTime);
    }

    private RaycastHit CameraInteractRaycast()
    {
        bool hitItem = SphereRayCast(itemLayer, out RaycastHit itemHit);

        if (hitItem)
        {
            isInteractabled = true;
            UpdateIndicator(itemHit);
            return itemHit;
        }

        bool hitInteraction = SphereRayCast(interactLayer, out RaycastHit interactHit);


        if (hitInteraction)
        {
            isInteractabled = true;
            UpdateIndicator(interactHit);
            return interactHit;
        }

        isInteractabled = false;
        UpdateIndicator(default);
        return default;
    }

    private bool SphereRayCast(LayerMask targetedLayer, out RaycastHit hit)
    {
        return Physics.SphereCast(
            cameraOffset.position,
            sphereRadius,
            cameraOffset.forward,
            out hit,
            interactDistance,
            targetedLayer
        );
    }

    private void UpdateIndicator(RaycastHit hit)
    {
        if (hit.collider == null) return;


        Indicator indicator = isInteractabled
            ? hit.collider.GetComponentInChildren<Indicator>()
            : null;

        if (indicator != curIndicator)
        {
            curIndicator?.SetInteractable(false);
            curIndicator = indicator;
        }

        curIndicator?.SetInteractable(indicator != null);
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 start = cameraOffset.transform.position;
        Vector3 end = start + cameraOffset.transform.forward * interactDistance;

        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(start, sphereRadius);
        Gizmos.DrawWireSphere(end, sphereRadius);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IImpactReceiver impactReceiver = hit.collider.GetComponentInParent<IImpactReceiver>();

        impactReceiver?.ReceiveImpact(IsRunning());

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.AddForceAtPosition(pushDir * playerPushForce, hit.point);
    }

    public void MoveCharacter(Vector3 moveDir) => charController.Move(moveDir * moveSpeedMultiplier * Time.deltaTime);

    public void MoveCamera(Vector2 newPosition) => cameraOffset.localPosition = new Vector3(0, newPosition.y, 0);

    public void ResetCameraPos() => cameraOffset.localPosition = initialCameraPos;

    public void RotateCamera(Quaternion newAngle) => cameraOffset.transform.localRotation = newAngle;

    public void SetCrouchHitbox()
    {
        charController.height = crouchHitboxHeight;
        charController.radius = crouchHitboxRadius;
        charController.center = new Vector3(0, crouchHitboxCenter, 0);

        detectionCollider.height = crouchDetectionHitboxHeight;
        detectionCollider.center = new Vector3(0, crouchDetectionHitboxPos, 0);
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

    public void SetPlayerPushForce(float newForce) => playerPushForce = newForce;
    public void ResetPlayerPushForce() => playerPushForce = defaultPlayerPushForce;
    public bool IsRunning() => Input.Player.Run.IsPressed();

    public void SetFOV(float targetFov)
    {
        if (fovCoroutine != null)
            StopCoroutine(fovCoroutine);

        fovCoroutine = StartCoroutine(FOVRoutine(targetFov));
    }

    private IEnumerator FOVRoutine(float targetFov)
    {
        float startFov = playerCamera.fieldOfView;
        float elapsed = 0f;

        while (elapsed < fovChangeDuration)
        {
            elapsed += Time.deltaTime;

            playerCamera.fieldOfView = Mathf.Lerp(
                startFov,
                targetFov,
                elapsed / fovChangeDuration);

            yield return null;
        }

        playerCamera.fieldOfView = targetFov;
        fovCoroutine = null;
    }

    public void ResetFOV()
    {
        SetFOV(DefaultFov);
    }
}
